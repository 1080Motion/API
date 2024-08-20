# Install and load necessary packages

# Packages to be installed:
# install.packages(c("httr", "jsonlite", "readr", "uuid", "base64enc", "stringr"))

# Load all packages
library(httr)
library(jsonlite)
library(readr)
library(uuid)
library(base64enc)
library(stringr)

# Initialize the API client
initialize_api_client <- function(api_host, api_key) {
  list(
    # Initialize base url and key
    base_url = api_host,
    api_key = api_key,
    session = httr::add_headers(`X-1080-API-Key` = api_key)
  )
}

# Function to perform GET requests
get <- function(api_client, relative_path) {
  url <- paste0(api_client$base_url, "/", relative_path)
  response <- httr::GET(url, config = api_client$session)
  stop_for_status(response)
  content <- httr::content(response, as = "text")
  jsonlite::fromJSON(content)
}

# Decode sample data from base64
decode_sample_data <- function(sample_data) {
  # Decode the base64 encoded string
  decoded_data <- base64enc::base64decode(sample_data)
  
  # Calculate the number of samples (each sample is 20 bytes)
  num_samples <- length(decoded_data) / 20
  
  # raw data -> raw vector
  raw_vector <- as.raw(decoded_data)
  unpacked_data <- readBin(raw_vector, what = "numeric", n = num_samples * 5, size = 4, endian = "little")
  
  # Extract the data
  time_since_start <- unpacked_data[seq(1, length(unpacked_data), by = 5)]
  position <- unpacked_data[seq(2, length(unpacked_data), by = 5)]
  speed <- unpacked_data[seq(3, length(unpacked_data), by = 5)]
  acceleration <- unpacked_data[seq(4, length(unpacked_data), by = 5)]
  force <- unpacked_data[seq(5, length(unpacked_data), by = 5)]
  
  # Return the results as a list
  list(time_since_start = time_since_start, position = position, speed = speed, acceleration = acceleration, force = force)
}

# Print all clients
print_all_clients <- function(api_client) {
  url <- 'Client'
  data <- get(api_client, url)
  if (length(data) == 0) {
    print("No clients found\n")
    return
  }
  print("Clients:\n")
  print(data)
}

# Print all exercises
print_all_exercises <- function(api_client, includePublic, includePrivate) {
  url <- sprintf('ExerciseType?includePublic=%s&includePrivate=%s', includePublic, includePrivate)
  data <- get(api_client, url)
  if (length(data) == 0) {
    print("No exercise types found\n")
    return
  }
  print("Exercise types:\n")
  print(data)
}

# Print sessions from today
print_sessions_from_today <- function(api_client) {
  # First get the list of sessions that are less than 1 day old
  url <- 'Session?maxAgeDays=1' # Change maxAgeDays to see sessions from more days
  
  sessions <- get(api_client, url)
  if (is.null(sessions) || (is.list(sessions) && length(sessions) == 0) || 
      (is.data.frame(sessions) && nrow(sessions) == 0)) {
    print("No sessions found today")
    return()
  }
  # Get list of clients so we can print the name of the client in the output (otherwise we only have the id)
  clients <- get(api_client, 'Client')
  
  print('Sessions from the last 24 hours:\n')
  for (i in seq_len(nrow(sessions))) {
    session <- sessions[i, ]
    
    # Access session details using the session ID
    session_id <- session$id  # Extract the session ID
    sessionDetails <- get(api_client, sprintf('Session/%s', session_id))
    if (is.null(sessionDetails))
    {
      print("ERROR: Could not get session details for session %s", session$id)
      continue
    }
    
    # Find the clientId that is corrensponding with the session
    client <- subset(clients, id == sessionDetails$clientId)
    
    if (nrow(client) > 0) {
      clientDisplayName <- client$displayName[1]  
    } else {
      clientDisplayName <- "Unknown Client"
    }
    
    print(sprintf("%s", session))
    if(is.null(client))
    {
      print("WARNING: No client found for session %s with clientId %s", sessionDetails$id, sessionDetails$clientId)
      continue
    }
    else
      clientDisplayName <- client$displayName
    
    print(sprintf("Session %s for client %s", sessionDetails$id, clientDisplayName))
    
    exercises <- sessionDetails$exercises
    
    # Ensure exercises is a data frame
    if (is.null(exercises) || nrow(exercises) == 0) {
      cat("No exercises\n")
      next
    }
    
    # Calculate total sets and print each exercise's information
    total_sets <- 0
    for (j in seq_len(nrow(exercises))) {
      exercise <- exercises[j, ]
      
      sets_list <- strsplit(as.character(exercise$sets), ", ")[[1]]
      sets_count <- length(sets_list)
      
      print(sprintf("Exercise %s (%d sets)\n", exercise$id, sets_count))
      total_sets <- total_sets + sets_count
    }
    
    print(sprintf("%d exercises, with %d sets in total\n", nrow(exercises), total_sets))
  }
}

# Download set training data to CSV
download_set_training_data_to_csv <- function(api_client, setId, filename) {
  url <- sprintf('TrainingData/Set/%s?includeSamples=false', setId)
  
  # Initialize the repNumber
  repNumber <- 0
  
  tryCatch({
    
    setResponse <- get(api_client, url)
    
    if (is.null(setResponse) || !("motionGroups" %in% names(setResponse))) {
      message(sprintf("No training data found for set %s", setId))
      return(NULL)
    }
    
    # Open file for writing
    fileConn <- file(filename, open = "w")
    
    # Write the CSV header
    writeLines("Set;RepNumber;RepId;RepTime;TopSpeed;TotalTime;Distance", con = fileConn)
    
    for (i in 1:nrow(setResponse$motionGroups)) {
      
      # Increment the repNumber
      repNumber <- repNumber + 1
      
      motionGroup <- setResponse$motionGroups[i, ]
      motionGroupId <- motionGroup$id
      motions <- motionGroup$motions
      created <- motionGroup$created
      if (is.list(motions)) {
        # Iterate through motions within the current motionGroup
        for (motion in motions) {
          
          # Extract motion data
          topSpeed <- motion$topSpeed
          totalTime <- motion$totalTime
          totalDistance <- motion$totalDistance
          
          # Create a CSV line for the current motion
          line <- sprintf("%s;%d;%s;%s;%f;%f;%f\n",
                          setId, 
                          repNumber, 
                          motionGroupId,
                          created,
                          topSpeed, 
                          totalTime, 
                          totalDistance)
          
          writeLines(line, con = fileConn)
        }
      } else {
        print("Error: motions is not a list at index", i, "\n")
      }
    }
    
    close(fileConn)
    
    file_path <- normalizePath(filename)
    print(sprintf("Training data saved to %s", file_path))
    
  }, error = function(e) {
    message(sprintf("An error occurred: %s", e$message))
  })
}

find_set_with_linear_training_data <- function(api_client) {
  numDays <- 1000
  url <- sprintf('Session?maxAgeDays=%d', numDays)
  
  sessions <- get(api_client, url)
  
  if (is.null(sessions) || length(sessions) == 0) {
    stop(sprintf("No sessions found in the last %d days", numDays))
  }
  
  # Get exercise types
  exerciseLibrary <- get(api_client, 'ExerciseType')
  target_type <- "Linear"
  
  if (is.null(exerciseLibrary) || length(exerciseLibrary) == 0) {
    stop("No exercise types found or unexpected format")
  }
  
  # Find exercises with "Linear" as presentation type
  linearExerciseTypes <- subset(exerciseLibrary, presentationType == target_type)
  
  # Iterate over sessions to find the set with linear training data
  for (i in seq_len(nrow(sessions))) {
    session <- sessions[i, ]  
    session_id <- session$id
    sessionDetails <- get(api_client, sprintf('Session/%s', session_id))
    
    if (is.null(sessionDetails) || !("exercises" %in% names(sessionDetails))) {
      next
    }
    
    exercises <- sessionDetails$exercises
    
    for (i in seq_len(nrow(exercises))) {
      exercise <- exercises[i, ]

      # Convert to dataframe
      if (is.null(exercise$sets)) {
        next
      } else if (is.list(exercise$sets)) {
        sets_df <- as.data.frame(do.call(rbind, exercise$sets))
        
      } else if (is.data.frame(exercise$sets)) {
        sets_df <- exercise$sets
        
      } else {
        next
      }
      
      if (nrow(sets_df) == 0) {
        next
      }
      
      # Check if exercise type matches any linear exercise types
      if (any(sapply(linearExerciseTypes$id, function(id) id == exercise$exerciseTypeId))) {
        return(sets_df[1, "id"])
      }
    }
  }
  
  stop(sprintf("No sessions within the last %d had any sets with linear training data", numDays))
}

# Download samples to CSV
download_samples_to_csv <- function(api_client, setId) {
  url <- sprintf("TrainingData/Set/%s?includeSamples=true", setId)
  setData <- get(api_client, url)
  
  if (length(setData) == 0) {
    cat(sprintf('No training data found for session %s\n', setId))
    return
  }
  
  repNumber <- 0
  cat('Data samples:\n')
  
  for (i in 1:nrow(setData$motionGroups)) {
    repNumber <- repNumber + 1
    
    motionGroup <- setData$motionGroups[i, ]
    
    filename <- sprintf("set-%s-rep-%d.csv", setId, repNumber)
    
    # Get the first motion since Linear only has one
    motion <- motionGroup$motions[[1]]
    
    # Decode the sample data
    samples <- decode_sample_data(motion$sampleData)
    
    if (is.null(samples)) {
      cat(sprintf("No samples found for rep %d in set %s\n", repNumber, setId))
      next
    }
    
    # Create a data frame with the headers and data
    data_df <- data.frame(
      Time = samples$time_since_start,
      Position = samples$position,
      Speed = samples$speed,
      Acceleration = samples$acceleration,
      Force = samples$force
    )
    
    # Write the data to a CSV file with headers
    write.table(data_df, file = filename, row.names = FALSE, sep = ";", dec = ".", col.names = TRUE)
    
    file_path <- normalizePath(filename)
    cat(sprintf('%s\n', filename))
    cat(sprintf("Saved to: %s\n", file_path))
  }
}

# Download set summary to CSV
download_set_summary_to_csv <- function(api_client) {
  set_id <- find_set_with_linear_training_data(api_client)
  download_set_training_data_to_csv(api_client, setId = set_id, filename = sprintf("set-summary-%s.csv", set_id))
}

# Download rep samples to CSV
download_rep_samples_to_csv <- function(api_client) {
  set_id <- find_set_with_linear_training_data(api_client)
  download_samples_to_csv(api_client, setId = set_id)
}

# Main function to run the tasks
main <- function() {
  api_client <- initialize_api_client("https://publicapi.1080motion.com/", "your-key-here")
  
  cat("***Retrieving clients***\n")
  print_all_clients(api_client)
  cat("\n")
  
  cat("***Retrieving exercise types***\n")
  print_all_exercises(api_client, includePublic = TRUE, includePrivate = FALSE)
  cat("\n")
  
  cat("***Retrieving sessions from today***\n")
  print_sessions_from_today(api_client)
  cat("\n")
  
  cat("***Downloading training data***\n")
  download_set_summary_to_csv(api_client)
  cat("\n")
  
  cat("***Downloading sample data***\n")
  download_rep_samples_to_csv(api_client)
  cat("\n")
}

# Run the main function
main()
