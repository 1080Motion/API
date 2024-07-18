﻿//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#nullable enable

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace WebApiSample.DataTransferObjects
{
    using System = global::System;

    

    /// <summary>
    /// Holds calculated peak or average values for a motion
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AggregatedValues
    {
        /// <summary>
        /// Speed, in meters per second (m/s)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("speed")]
        public double Speed { get; set; } = default!;

        /// <summary>
        /// Force, in newtons (N).
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("force")]
        public double Force { get; set; } = default!;

        /// <summary>
        /// Power, in watts (W)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("power")]
        public double Power { get; set; } = default!;

        /// <summary>
        /// Acceleration, in meters per second per second (m/s2)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("acceleration")]
        public double Acceleration { get; set; } = default!;

    }

    /// <summary>
    /// The different ways training data can be presented in the 1080 software and web applications.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum ExercisePresentationType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Repetitions")]
        Repetitions = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Linear")]
        Linear = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"ChangeOfDirection")]
        ChangeOfDirection = 2,

    }

    /// <summary>
    /// Represents a measurement of height and weight for a client at a specific date
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class HeightAndWeightMeasurement
    {
        /// <summary>
        /// Date when the measurement was done
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("entryDate")]
        public System.DateTimeOffset EntryDate { get; set; } = default!;

        /// <summary>
        /// Height of the client, in cm. (1 centimeter = 0.01m)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("height")]
        public double Height { get; set; } = default!;

        /// <summary>
        /// Weight of the client, in kg.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("weight")]
        public double Weight { get; set; } = default!;

    }

    /// <summary>
    /// Which phase to show when viewing the bar chart of a rep style exercise.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum PhaseDisplayMode
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Concentric")]
        Concentric = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Eccentric")]
        Eccentric = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"Both")]
        Both = 2,

    }

    /// <summary>
    /// Represents a client in the database. All training data is logged on a client
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicClient
    {
        /// <summary>
        /// Unique id for this entity
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("created")]
        public System.DateTimeOffset Created { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was last edited
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("edited")]
        public System.DateTimeOffset? Edited { get; set; } = default!;

        /// <summary>
        /// Name of the client.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("displayName")]
        public string? DisplayName { get; set; } = default!;

        /// <summary>
        /// Height of the client, in cm. (1 centimeter = 0.01m)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("height")]
        public float Height { get; set; } = default!;

        /// <summary>
        /// Current weight of the client, in kg.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("weight")]
        public float Weight { get; set; } = default!;

        /// <summary>
        /// Optional date of birth.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("dateOfBirth")]
        public System.DateTimeOffset? DateOfBirth { get; set; } = default!;

        /// <summary>
        /// Historical measurements of body weight and height for this client
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("historicalMeasurements")]
        public System.Collections.Generic.IList<HeightAndWeightMeasurement>? HistoricalMeasurements { get; set; } = default!;

    }

    /// <summary>
    /// Represents a single data sample acquired from the machine. These are always data samples are returned serialized as a base64 encoded byte array as
    /// <br/>described here https://github.com/1080Motion/API/wiki/DataSamples-(web-api)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicDataSample
    {
        /// <summary>
        /// Time since the start of the motion, in seconds
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("timeSinceStart")]
        public float TimeSinceStart { get; set; } = default!;

        /// <summary>
        /// Position of this sample. The unit is meters but the absolute value is machine specific
        /// <br/>(on sprint1/quantum it's relative to the most recent calibration point but that may not necessarily be true for future versions of the machines)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("position")]
        public float Position { get; set; } = default!;

        /// <summary>
        /// The speed, in meters per second (m/s)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("speed")]
        public float Speed { get; set; } = default!;

        /// <summary>
        /// Acceleration (in meters per second per second, m/s2).
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("acceleration")]
        public float Acceleration { get; set; } = default!;

        /// <summary>
        /// Measured force in Newtons (N).
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("force")]
        public float Force { get; set; } = default!;

    }

    /// <summary>
    /// An exercise is a grouping of reps of the same exercise type inside a session.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicExercise
    {
        /// <summary>
        /// Unique id for this entity
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("created")]
        public System.DateTimeOffset Created { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was last edited
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("edited")]
        public System.DateTimeOffset? Edited { get; set; } = default!;

        /// <summary>
        /// The type of exercise.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("exerciseTypeId")]
        public System.Guid ExerciseTypeId { get; set; } = default!;

        /// <summary>
        /// Name of the exercise type
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("exerciseTypeName")]
        public string? ExerciseTypeName { get; set; } = default!;

        /// <summary>
        /// List of sets performed for this exercise in the current session
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("sets")]
        public System.Collections.Generic.IList<PublicSet>? Sets { get; set; } = default!;

    }

    /// <summary>
    /// Represents an exercise type from the exercise library.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicExerciseType
    {
        /// <summary>
        /// Unique id for this entity
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("created")]
        public System.DateTimeOffset Created { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was last edited
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("edited")]
        public System.DateTimeOffset? Edited { get; set; } = default!;

        /// <summary>
        /// Name given to this exercise type
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string? Name { get; set; } = default!;

        /// <summary>
        /// Optional instructions for this exercise
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("instructions")]
        public string? Instructions { get; set; } = default!;

        /// <summary>
        /// True if the exercise separates left from right
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("isBilateral")]
        public bool IsBilateral { get; set; } = default!;

        /// <summary>
        /// True if this is a public exercise type (accessible to all 1080 users) or if it is private to the current instructor.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; } = default!;

        /// <summary>
        /// Name of the arch type for this exercise.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("archTypeName")]
        public string? ArchTypeName { get; set; } = default!;

        /// <summary>
        /// Display name version of the TEM.PublicWebApi.Definitions.Dto.PublicExerciseType.ArchTypeName. This is the name that gets displayed to the user
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("archTypeDisplayName")]
        public string? ArchTypeDisplayName { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("presentationType")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public ExercisePresentationType PresentationType { get; set; } = default!;

    }

    /// <summary>
    /// The motion contains information about the actual work done. Both in term of aggregated Average/Peak values as well as
    /// <br/>the list of data samples recorded from the machine.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicMotion
    {

        [System.Text.Json.Serialization.JsonPropertyName("resistanceValues")]
        public Resistance ResistanceValues { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("averageValues")]
        public AggregatedValues AverageValues { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("peakValues")]
        public AggregatedValues PeakValues { get; set; } = default!;

        /// <summary>
        /// Total moved distance (in meters) for this motion
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("totalDistance")]
        public double TotalDistance { get; set; } = default!;

        /// <summary>
        /// Total time (in seconds) for this motion
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("totalTime")]
        public double TotalTime { get; set; } = default!;

        /// <summary>
        /// Calculated top speed, in m/s. Only set for linear and CoD exercises
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("topSpeed")]
        public float? TopSpeed { get; set; } = default!;

        /// <summary>
        /// True if the motion is eccentric (moving towards the machine), false otherwise
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("isEccentric")]
        public bool IsEccentric { get; set; } = default!;

        /// <summary>
        /// Stream of data samples that comprise this motion.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("sampleData")]
        public byte[]? SampleData { get; set; } = default!;

    }

    /// <summary>
    /// Represents a single rep in a set (or run for sprint)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicMotionGroup
    {
        /// <summary>
        /// Unique id for this entity
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("created")]
        public System.DateTimeOffset Created { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was last edited
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("edited")]
        public System.DateTimeOffset? Edited { get; set; } = default!;

        /// <summary>
        /// Color, in rgb hex format (for example FFC627 or 404040)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("color")]
        public string? Color { get; set; } = default!;

        /// <summary>
        /// User entered color for the rep
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("comment")]
        public string? Comment { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("side")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public Side Side { get; set; } = default!;

        /// <summary>
        /// Motions for this rep
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("motions")]
        public System.Collections.Generic.IList<PublicMotion>? Motions { get; set; } = default!;

    }

    /// <summary>
    /// Represents a training session
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicSession
    {
        /// <summary>
        /// Unique id for this entity
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("created")]
        public System.DateTimeOffset Created { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was last edited
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("edited")]
        public System.DateTimeOffset? Edited { get; set; } = default!;

        /// <summary>
        /// Id of the client that performed the training session
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("clientId")]
        public System.Guid ClientId { get; set; } = default!;

        /// <summary>
        /// List of exercises in the session
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("exercises")]
        public System.Collections.Generic.IList<PublicExercise>? Exercises { get; set; } = default!;

    }

    /// <summary>
    /// A set is the container of the actual reps.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicSet
    {
        /// <summary>
        /// Unique id for this entity
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("created")]
        public System.DateTimeOffset Created { get; set; } = default!;

        /// <summary>
        /// Date and time when the entity was last edited
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("edited")]
        public System.DateTimeOffset? Edited { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("viewSettings")]
        public SetViewSettings ViewSettings { get; set; } = default!;

        /// <summary>
        /// Configured external load. Only applicable for synchro smith exercises
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("externalLoad")]
        public int ExternalLoad { get; set; } = default!;

    }

    /// <summary>
    /// Holds the training data for a set (reps)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class PublicSetData
    {
        /// <summary>
        /// Id of the set that the data belongs to
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("setId")]
        public System.Guid SetId { get; set; } = default!;

        /// <summary>
        /// List of reps for this set
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("motionGroups")]
        public System.Collections.Generic.IList<PublicMotionGroup>? MotionGroups { get; set; } = default!;

    }

    /// <summary>
    /// The machine settings at the time of training
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Resistance
    {
        /// <summary>
        /// Amount of concentric load set on the machine (in kg)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("concentricLoad")]
        public float ConcentricLoad { get; set; } = default!;

        /// <summary>
        /// Amount of eccentric load set on the machine (in kg)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("eccentricLoad")]
        public float EccentricLoad { get; set; } = default!;

        /// <summary>
        /// The mode the machine was in. E.g. "NFW", "Isotonic"
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("mode")]
        public string? Mode { get; set; } = default!;

        /// <summary>
        /// The gear. One of 1 or 2
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("gear")]
        public int Gear { get; set; } = default!;

        /// <summary>
        /// The concentric speed limit set on the machine (in m/s).
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("concentricSpeedLimit")]
        public float ConcentricSpeedLimit { get; set; } = default!;

        /// <summary>
        /// The eccentric speed limit set on the machine (in m/s).
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("eccentricSpeedLimit")]
        public float EccentricSpeedLimit { get; set; } = default!;

    }

    /// <summary>
    /// Basic information about a training session
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SessionInfo
    {
        /// <summary>
        /// Unique id of the session
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Time when the session was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("timestamp")]
        public System.DateTimeOffset Timestamp { get; set; } = default!;

        /// <summary>
        /// Id of the client that the session is for
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("clientId")]
        public System.Guid ClientId { get; set; } = default!;

    }

    /// <summary>
    /// Settings controlling how the data of a set is presented
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SetViewSettings
    {

        [System.Text.Json.Serialization.JsonPropertyName("phaseDisplayMode")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public PhaseDisplayMode PhaseDisplayMode { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("unit")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public UnitKind Unit { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("xUnit")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public UnitKind XUnit { get; set; } = default!;

    }

    /// <summary>
    /// The side of a rep.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum Side
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Unknown")]
        Unknown = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Left")]
        Left = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"Right")]
        Right = 2,

    }

    /// <summary>
    /// Data for a single split inside a split report
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SplitData
    {
        /// <summary>
        /// Start position
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("start")]
        public double Start { get; set; } = default!;

        /// <summary>
        /// End position
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("end")]
        public double End { get; set; } = default!;

        /// <summary>
        /// Time for the split, in seconds
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("time")]
        public double Time { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("averages")]
        public AggregatedValues Averages { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("peaks")]
        public AggregatedValues Peaks { get; set; } = default!;

    }

    /// <summary>
    /// A split report for a single run
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SplitReport
    {
        /// <summary>
        /// The motion group (run) that this split report is for
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("motionGroupId")]
        public System.Guid MotionGroupId { get; set; } = default!;

        /// <summary>
        /// Split data
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("splits")]
        public System.Collections.Generic.IList<SplitData>? Splits { get; set; } = default!;

        /// <summary>
        /// Configured split length, in meters
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("splitLength")]
        public double SplitLength { get; set; } = default!;

        /// <summary>
        /// True if the lengths are in yards, false if in meters
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("isYards")]
        public bool IsYards { get; set; } = default!;

    }

    /// <summary>
    /// The different units supported
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum UnitKind
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Position")]
        Position = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Speed")]
        Speed = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"Acceleration")]
        Acceleration = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"Force")]
        Force = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"Power")]
        Power = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"Time")]
        Time = 5,

    }



    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string? Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string? response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception? innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string? response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception? innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625