# **ML 24/25-03 Implement Anomaly Detection Sample, Team Synergy**

This project demonstrates how to implement anomaly detection using the NeoCortexApi, a .NET library for Hierarchical Temporal Memory (HTM). HTM is a machine learning framework inspired by the human neocortex, designed for sequence learning, prediction, and anomaly detection.

This sample project is built using .NET 9 and leverages the NeoCortexApi to create a model capable of detecting anomalies in time-series data. Whether you're working with sensor data, financial transactions, or any other sequential data, this project provides a foundation for understanding how HTM can be applied to identify unusual patterns or outliers.

## What is HTM and NeoCortexApi?

Hierarchical Temporal Memory (HTM) is a biologically inspired machine learning framework that mimics the structure and function of the human neocortex. It excels at learning patterns in streaming data and making predictions based on temporal sequences. HTM is particularly well-suited for anomaly detection, as it can identify deviations from learned patterns in real-time.

The NeoCortexApi is a .NET implementation of HTM, providing a powerful and flexible framework for building HTM-based applications. It includes tools for encoding data, training models, and making predictions, making it accessible for developers to integrate HTM into their projects.

## Requirements

To run this project, we need.
* [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* Nuget package: [NeoCortexApi Version= 1.1.5](https://www.nuget.org/packages/NeoCortexApi/)

For code debugging, we recommend using visual studio 2022/visual studio code IDE. 

## Project Workflow
The project follows these steps:

*Command-Line Arguments*: Parse input parameters such as file paths and tolerance values.

*File Handling*: Read and validate training and inferring data files.

*Data Parsing*: Parse CSV files into sequences of numerical data.

*Sequence Analysis*: Analyze sequences to find the maximum value for encoding.

*HTM Training*: Train the HTM model using the training sequences.

*Anomaly Detection*: Detect anomalies in the inferring sequences using the trained model.

## Step-by-Step Explanation

### Step 1: Command-Line Arguments

The project starts by parsing command-line arguments to configure the anomaly detection system. The [ConsoleArgumentsHandler](https://github.com/Muhammad-Talha-S/synergy-neocortex-ad/blob/master/source/MySEProject/AnomalyDetectionTeamSynergy/ConsoleArgumentsHandler.cs) class handles this by extracting values for parameters such as file paths, folder paths, and numerical settings.

#### Key Parameters and Default Values

* *N*: Trims the first N elements of inferring sequences. (Default: 0)
* *trainingFile*: Path to the training data file. (Default: Empty string)
* *inferringFile*: Path to the inferring (testing) data file. (Default: Empty string)
* *trainingFolder*: Path to the folder containing multiple training files. (Default: Empty string)
* *inferringFolder*: Path to the folder containing multiple inferring files. (Default: Empty string)
* *toleranceValue*: Sets the sensitivity threshold for anomaly detection. (Default: 0.1)

If no arguments are provided, the application uses the above defaults.

#### Usage Example

Run the application with command-line arguments as follows:

```bash
dotnet run --n 5 --training-file "data/training.csv" --inferring-file "data/inferring.csv" --tolerance 0.1
```

### Step 2: File Handling

The [FileHandler](https://github.com/Muhammad-Talha-S/synergy-neocortex-ad/blob/master/source/MySEProject/AnomalyDetectionTeamSynergy/FileHandler.cs) class manages file operations for training and inferring data. It gathers, validates, and filters CSV files from specified paths or default folders.

#### Key Features:
* *Default Folders*: Automatically uses TrainingData and InferringData folders in the project's base directory if no paths are provided.

* *File Validation*: Ensures files exist and have a .csv extension.

* *Flexible Input*: Accepts individual files or folders for both training and inferring data.

```csharp
private List<string> ValidateAndFilterFiles(List<string> files)
{
    var validFiles = new List<string>();

    foreach (var file in files)
    {
        if (!File.Exists(file))
        {
            Console.WriteLine($"Warning: File not found - {file}");
            continue;
        }

        if (!IsCsv(file))
        {
            Console.WriteLine($"Warning: File is not a CSV - {file}");
            continue;
        }

        validFiles.Add(file);
    }

    return validFiles;
}
```

#### Usage:

* *Training Data*: Provide a specific file (--training-file) or folder (--training-folder). If none are provided, it defaults to the TrainingData folder.

* *Inferring Data*: Provide a specific file (--inferring-file) or folder (--inferring-folder). If none are provided, it defaults to the InferringData folder.


### Step-3 CSV Handling and HTM Input

This step involves reading, parsing, and transforming CSV data into a format suitable for HTM (Hierarchical Temporal Memory) training and inference. The CSVHandler class handles CSV file operations, while the CSVToHTMInput class converts sequences into HTM-compatible input.

#### Key Features:

*CSV Parsing*:

* Reads CSV files and extracts numerical sequences.

* Skips invalid lines and sequences with fewer than 3 values.

* Supports trimming sequences by removing the first N elements.

```csharp
public List<List<double>> ParseSequencesFromCSV(string filePath)
{
    var sequences = new List<List<double>>();
    var lines = File.ReadAllLines(filePath);

    for (int i = 1; i < lines.Length; i++) // Skip header
    {
        var values = lines[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (values.All(v => double.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out _)))
        {
            var sequence = values.Select(v => double.Parse(v, CultureInfo.InvariantCulture)).ToList();
            if (sequence.Count > 2) sequences.Add(sequence); // Skip sequences with < 3 values
        }
    }
    return sequences;
}

public List<List<double>> TrimSequences(List<List<double>> sequences, int N)
{
    return sequences.Select(seq => seq.Skip(N).ToList()).ToList();
}
```

*HTM Input Transformation*:

* Converts sequences into a dictionary format required for HTM training.

* Assigns unique keys to each sequence for identification.

```csharp
public Dictionary<string, List<double>> BuildHTMInput(List<List<double>> sequences, string keyPrefix = "S")
{
    var dictionary = new Dictionary<string, List<double>>(sequences.Count);
    for (int i = 0; i < sequences.Count; i++)
    {
        dictionary[$"{keyPrefix}{i + 1}"] = sequences[i]; // Assign unique keys
    }
    return dictionary;
}
```

*Data Saving*:

* Saves inferring sequences, predicted values, and best-matched sequences to a CSV file for analysis.

```csharp
public void SaveToCsv(string csvFileName, List<double> inferringSequence, List<string> predictedValues, List<double> bestMatchedSequence)
{
    string folderPath = Path.Combine(projectBaseDirectory, "ModelPredictions");
    Directory.CreateDirectory(folderPath);

    using (StreamWriter writer = new StreamWriter(Path.Combine(folderPath, csvFileName)))
    {
        writer.WriteLine("Inferring Sequence,Predicted Values,Best Matched Sequence");
        for (int i = 0; i < Math.Max(inferringSequence.Count, predictedValues.Count); i++)
        {
            writer.WriteLine($"{inferringSequence[i]},{predictedValues[i]},{bestMatchedSequence[i]}");
        }
    }
}
```



## Resources
[NeoCortexApi GitHub Repository](https://github.com/ddobric/neocortexapi)

[Numenta](https://www.numenta.com/resources/htm/htmschool/)