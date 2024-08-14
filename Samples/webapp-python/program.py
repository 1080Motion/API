from operator import setitem
from re import S
from tkinter import NO
import requests
import os
import json
import base64
import struct
from uuid import UUID
from typing import Optional

from objects import (
    AggregatedValues,
    HeightAndWeightMeasurement,
    PublicClient,
    PublicDataSample,
    PublicExerciseType,
    Resistance,
    PublicMotion,
    PublicMotionGroup,
    PublicSet,
    PublicSetData,
    PublicExercise,
    PublicSession,
    SessionInfo
)

class ApiClient:
    def __init__(self, api_host: str, api_key: str):
        # Initiates base url and stores the key
        self.base_url = api_host
        self.api_key = api_key
        # Creates a new session for requests
        self.session = requests.Session()
        # Update headers to replace the key for authentication
        self.session.headers.update({
            "X-1080-API-Key": api_key
        })
        
    def get(self, relative_path: str):
        # Appends relative path to base url and sends GET request
        url = f"{self.base_url}/{relative_path}"
        response = self.session.get(url)
        response.raise_for_status()
        return response.json() 

    def _decode_sample_data(self, sample_data):
        decoded_data = base64.b64decode(sample_data)
    
        # Use struct.unpack to unpack all data at once
        n_samples = len(decoded_data) // 20
        unpacked_data = struct.unpack(f"<{n_samples * 5}f", decoded_data)
    
        # Extract the individual parts using slicing
        time_since_start = unpacked_data[0::5]
        position = unpacked_data[1::5]
        speed = unpacked_data[2::5]
        acceleration = unpacked_data[3::5]
        force = unpacked_data[4::5]

        return time_since_start, position, speed, acceleration, force

    def print_all_clients(self):
        url = 'Client'
        try:
            # Get client data from the API
            data = self.get(url)
            if not data:
                print("No clients found")
                return

            print("Clients:")
            # Convert into client object class
            clients = [PublicClient(**item) for item in data]
            for client in clients:
                print(client)
                
        except requests.RequestException as e:
            print(f"An error occurred: {e}")
        except Exception as e:
            print(f"An error occurred: {e}")
            
    def print_all_exercises(self, includePublic : bool, includePrivate : bool):
        # Get all exercises and deserialize them into an array of PublicExerciseType objects.
        # The parameters passed to this function is passed directly to the API in the HTTP request.
        url = f'ExerciseType?includePublic={includePublic}&includePrivate={includePrivate}'
        try:
            data = self.get(url)
            if not data:
                print("No exercise types found")
                return
            
            print("Exercise types:")
            # Deserialize JSON into PublicExerciseType object
            exerciseTypes = [PublicExerciseType(**item) for item in data]
            for exerciseType in exerciseTypes:
                print(exerciseType)
                
        except requests.RequestException as e:
            print(f"An error occured while retrieving exercise types: {e}")
        except Exception as e:
            print(f"An error occured: {e}")
            
    # Retrieve data and display without converting into object classes
    def print_sessions_from_today(self):
        # First get the list of sessions that are less than 1 day old
        url = 'Session?maxAgeDays=300' # Change maxAgeDays to see sessions from more days
        try:
            sessions = self.get(url)
            if not sessions:
                print("No sessions found today")
                return
    
            # Get list of clients so we can print the name of the client in the output (otherwise we only have the id)
            clients = self.get('Client')
    
            print("Sessions from the last 24 hours:")
            for session in sessions:
                # Download more information for this session. This includes the list of exercises and sets inside the session
                sessionDetails = self.get(f'Session/{session["id"]}')
                if not sessionDetails:
                    # This should never happen since we just got the session info from the same API
                    print(f"ERROR: Could not get session details for session {session['id']}")
                    continue
                
                # Find the client that corresponds with the session
                client = next((c for c in clients if c['id'] == sessionDetails['clientId']), None)
                if client is None:
                    # If no client is found for the current session
                    print(f"WARNING: No client found for session {sessionDetails['id']} with clientId {sessionDetails['clientId']}")
                    clientDisplayName = "Unknown Client"
                else:
                    clientDisplayName = client.get('displayName', 'Unknown Client')
        
                print(f"Session {sessionDetails['id']} for client {clientDisplayName}")
                exercises = sessionDetails.get('exercises', [])
                if not exercises:
                    print("No exercises")
                    continue
                
                # Print out data
                total_sets = sum(len(e.get('sets', [])) for e in exercises)
                print(f"{len(exercises)} exercises, with {total_sets} sets in total")
                for exercise in exercises:
                    print(f"Exercise {exercise['id']} ({len(exercise.get('sets', []))} sets)")
    
        except Exception as e:
            print(f"An error occurred: {e}")
            

    def download_set_training_data_to_csv(self, setId: UUID, filename: str):
        # Writes training data to file
        url = f'TrainingData/Set/{setId}?includeSamples=false'
        repNumber = 0
        
        try:
            setResponse = self.get(url)
            # Check if response contains expected data
            if not setResponse or 'motionGroups' not in setResponse:
                print(f"No training data found for set {setId}")
                return
            
            with open(filename, 'w') as writer:          
                writer.write("Set;RepNumber;RepId;RepTime;TopSpeed;TotalTime;Distance\n")
                
                for motionGroup in setResponse.get('motionGroups', []):
                    if 'motions' not in motionGroup or not motionGroup['motions']:
                        continue
                    
                    for motion in motionGroup['motions']:
                        repNumber += 1
                        # Create a CSV line for the current motion
                        line = (f"{setId};{repNumber};{motionGroup['id']};"
                                f"{motionGroup.get('created', '')};"
                                f"{motion.get('topSpeed', '')};"
                                f"{motion.get('totalTime', '')};"
                                f"{motion.get('totalDistance', '')}\n")
                        writer.write(line)
            file_path = os.path.abspath(filename)
            print(f'Training data saved to {file_path}')
            
        except Exception as e:
            print(f"An error occurred: {e}")

    
    def find_set_with_linear_training_data(self):
        numDays = 100
        url = f'Session?maxAgeDays={numDays}'
        try:
            sessions = self.get(url)
            if not sessions:
                 raise RuntimeError(f"No sessions found in the last {numDays} days")
            
            # Get library of exercise types
            exerciseLibrary = self.get('ExerciseType')
            target_type = "Linear"
            
            # Find exercises with linear as presentation type
            linearExerciseTypes = [e for e in exerciseLibrary if e.get('presentationType') == target_type]
            
            for session in sessions:
                session_id = session.get('id')
                details = self.get(f"Session/{session_id}")
                if not details:
                    continue
                
                for exercise in details["exercises"]:
                    if not exercise["sets"]:
                        continue
                    if not any(et['id'] == exercise['exerciseTypeId'] for et in linearExerciseTypes):
                        continue
                    return exercise['sets'][0]['id'] if exercise['sets'] else None
                
            raise RuntimeError(f'No sessions within the last {numDays} had any sets')
        
        except Exception as e:
            print(f"An error occurred: {e}")
            
    def download_samples_to_csv(self, setId : UUID):
            # Get training data with samples included    
            setData = self.get(f"TrainingData/Set/{setId}?includeSamples=true")
            if not setData:
                print(f'No training data found for session {setId}')
                return
            
            repNumber = 0
            print('Data samples:')
            for motionGroup in setData.get('motionGroups', []):
                if 'motions' not in motionGroup or not motionGroup['motions']:
                            continue
                repNumber += 1
                # Name the file with setId and rep number
                filename = f"set-{setId}-rep-{repNumber}.csv"
                motion = motionGroup.get("motions")[0]
                samples = self._decode_sample_data(motion.get("sampleData"))
                if not samples:
                    print(f"No samples found for rep {repNumber} in set {setId}")
                    
                for sample in samples:
                    with open(filename, 'w') as writer:
                        writer.write("Time;Position;Speed;Acceleration;Force\n")
                        # Write each line to CSV
                        for i in range(len(samples[0])): 
                            writer.write(f"{samples[0][i]};{samples[1][i]};{samples[2][i]};{samples[3][i]};{samples[4][i]}\n")
                    
                file_path = os.path.abspath(filename)
                print(f'{filename}')
            print(f"Saved to: {file_path}")            
            return
    
    def download_set_summary_to_csv(self):
        set_id = self.find_set_with_linear_training_data()
        self.download_set_training_data_to_csv(setId = set_id, filename=f"set-summary-{set_id}.csv")
        
    def download_rep_samples_to_csv(self):
        set_id = self.find_set_with_linear_training_data()
        self.download_samples_to_csv(setId=set_id)
    



def main():
       # Setup ApiClient with url and key   
       api_client = ApiClient("https://publicapi.1080motion.com/", "your-key-here")

       print("***Retrieving clients***")
       api_client.print_all_clients()
       print("\n")
       
       print("***Retrieving exercise types***")
       api_client.print_all_exercises(includePublic=True, includePrivate=False)
       print("\n")
       
       print("***Retrieving sessions from today***")
       api_client.print_sessions_from_today()
       print('\n')
       
       # Download some training data and save it to csv files
       print('***Downloading set training data***')
       api_client.download_set_summary_to_csv()
       print('\n')
       print('***Downloading samples to csv***')
       api_client.download_rep_samples_to_csv()

if __name__ == "__main__":
    main()
   
