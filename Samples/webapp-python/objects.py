
from dataclasses import dataclass, field
from datetime import datetime
from email.policy import default
from enum import Enum
from optparse import Option
from tkinter import N
from typing import List, Optional
from uuid import UUID
from datetime import datetime

# Object classes can be made manually or by JSON to object class converters.
# Example of a JSON to Python converter website: https://codebeautify.org/json-to-python-pojo-generator

#Enums

class ExercisePresentationType(Enum):
    REPETITIONS = "Repetitions"
    LINEAR = "Linear"
    CHANGE_OF_DIRECTION = "ChangeOfDirection"

class PhaseDisplayMode(Enum):
    CONCENTRIC = "Concentric"
    ECCENTRIC = "Eccentric"
    BOTH = "Both"

class Side(Enum):
    LEFT = "Left"
    RIGHT = "Right"
    BOTH = "Both"


    # Data Classes

@dataclass
class AggregatedValues:
    speed: float
    force: float
    power: float
    acceleration: float

@dataclass
class HeightAndWeightMeasurement:
    entry_date: datetime
    height: float
    weight: float

@dataclass
class PublicClient:
    id: UUID
    created: datetime
    height: float
    weight: float
    edited: Optional[datetime] = None
    displayName: Optional[str] = None
    dateOfBirth: Optional[datetime] = None
    historicalMeasurements: Optional[List[HeightAndWeightMeasurement]] = field(default_factory=list)

@dataclass
class PublicDataSample:
    time_since_start: float
    position: float
    speed: float
    acceleration: float
    force: float

@dataclass
class PublicExerciseType:
    id: UUID
    isBilateral: bool
    isPublic: bool
    presentationType: ExercisePresentationType
    created: datetime
    edited: Optional[datetime] = None
    name: Optional[str] = None
    instructions: Optional[str] = None
    archTypeName: Optional[str] = None
    archTypeDisplayName: Optional[str] = None
    

@dataclass
class Resistance:
    EccentricLoad: float
    Mode : str
    Gear : int
    ConcentricSpeedLimit : float
    EccentricSpeedLimit : float
    
@dataclass
class SessionInfo:
    id: UUID
    timestamp : datetime
    clientId : UUID

@dataclass
class PublicMotion:
    resistanceValues: Resistance
    averageValues: AggregatedValues
    peakValues: AggregatedValues
    totalDistance: float
    totalTime: float
    isEccentric: bool
    topSpeed: Optional[float] = None
    sampleData: Optional[bytes] = None

@dataclass
class PublicMotionGroup:
    id: UUID
    created: datetime
    side: Side
    edited: Optional[datetime] = None
    color: Optional[str] = None
    comment: Optional[str] = None
    motions: Optional[List[PublicMotion]] = field(default_factory=list)

@dataclass
class PublicSet:
    id: UUID
    created: datetime
    externalLoad: int
    edited: Optional[datetime] = None

@dataclass
class PublicSetData:
    setId: UUID
    motionGroups: Optional[List[PublicMotionGroup]] = field(default_factory=list)

@dataclass
class PublicExercise:
    id: UUID
    created: datetime
    exerciseTypeId: UUID
    edited: Optional[datetime] = None
    exerciseTypeName: Optional[str] = None
    sets: Optional[List[PublicSet]] = field(default_factory=list)

@dataclass
class PublicSession:
    id: UUID
    created: datetime
    clientId: UUID
    exercises: Optional[List[PublicExercise]] = field(default_factory=list)
    edited: Optional[datetime] = None
    client_id : Optional[UUID] = None