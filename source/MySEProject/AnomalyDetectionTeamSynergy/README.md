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

## Resources
[NeoCortexApi GitHub Repository](https://github.com/ddobric/neocortexapi)

[Numenta](https://www.numenta.com/resources/htm/htmschool/)