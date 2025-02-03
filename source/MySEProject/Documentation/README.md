# **Implement Anomaly Detection**
Ref: *ML 24/25-03*  

## Introduction

HTM (Hierarchical Temporal Memory) is a machine learning algorithm, which uses a hierarchical network of nodes to process time-series data in a distributed way. Each nodes, or columns, can be trained to learn, and recognize patterns in input data. This can be used in identifying anomalies/deviations from normal patterns. It is a promising approach for anomaly detection and prediction in a variety of applications. In this project, multisequencelearning class in NeoCortex API is used to implement an anomaly detection system, such that numerical sequences are read from multiple csv files inside a folder, train project HTM Engine, and use the trained engine for learning patterns and detect anomalies.  

# Requirements

To run this project, we need.
* [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* Nuget package: [NeoCortexApi Version= 1.1.5](https://www.nuget.org/packages/NeoCortexApi/)

For code debugging, we recommend using visual studio 2022/visual studio code IDE. 

# Usage

To run this project, 

* Install .NET SDK. Then using code editor/IDE, create a new console project and place all the C# codes inside your project folder. 
* Add/reference nuget package NeoCortexApi v1.1.5 to this project.
* Place numerical sequence CSV Files (datasets) under relevant folders respectively. All the folders should be inside the project folder. More details given below.

This project is based on NeoCortex API. More details [NeoCortexAPI](https://github.com/ddobric/neocortexapi/blob/master/source/Documentation/gettingStarted.md).

# Details

We have used [MultiSequenceLearning](https://github.com/ddobric/neocortexapi/blob/master/source/Samples/NeoCortexApiSample/MultisequenceLearning.cs) class in NeoCortex API for training our HTM Engine. We are going to start by reading and using data from both our training (learning) folder (present as numerical sequences in CSV Files in 'training' folder inside project directory) and predicting folder (present as numerical sequences in CSV Files in 'predicting' folder inside project directory) to train HTM Engine. For testing purposes, we are going to read numerical sequence data from predicting folder and remove the first few elements (essentially, making it subsequence of the original sequence; we already added anomalies in this data at random indexes), and then use it to detect anomalies.

Please note that all files are read with .csv extension inside the folders, and exception handlers are in place if the format of the files are not in proper order.

For this project, we are using artificial integer sequence data of network load (rounded off to nearest integer, in precentage), which are stored inside the csv files. Example of a csv file within training folder.

```
49,52,55,48,52,47,46,50,52,47
49,52,55,48,52,47,46,50,49,47
.............................
.............................
48,54,55,48,52,47,46,50,49,45
51,54,55,48,52,47,46,50,49,45
```
Normally, the values stay within the range of 45 to 55. For testing, we consider anything outside this range to be an anomaly. We have uploaded the graphs of our data in this repository for reference. 

1. Graph for numerical sequence data from training folder (without anomalies) can be found [here](https://github.com/SouravPaulSumit/Team_anomaly/blob/master/mySEProject/AnomalyDetectionSample/output/graph_of_data_training_folder.jpg).
2. Graph of combined numerical sequence data from training folder (without anomalies) and predicting folder (with anomalies) can be found [here](https://github.com/SouravPaulSumit/Team_anomaly/blob/master/mySEProject/AnomalyDetectionSample/output/combined_data_training_and_predicting_folder.jpg).
