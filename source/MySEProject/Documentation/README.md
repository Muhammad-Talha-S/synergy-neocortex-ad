# **Implement Anomaly Detection**
Ref: *ML 24/25-03*  

## Introduction

HTM (Hierarchical Temporal Memory) is a machine learning algorithm, which uses a hierarchical network of nodes to process time-series data in a distributed way. Each nodes, or columns, can be trained to learn, and recognize patterns in input data. This can be used in identifying anomalies/deviations from normal patterns. It is a promising approach for anomaly detection and prediction in a variety of applications. In this project, multisequencelearning class in NeoCortex API is used to implement an anomaly detection system, such that numerical sequences are read from multiple csv files inside a folder, train project HTM Engine, and use the trained engine for learning patterns and detect anomalies.  

# Requirements

To run this project, we need.
* [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* Nuget package: [NeoCortexApi Version= 1.1.4](https://www.nuget.org/packages/NeoCortexApi/)

For code debugging, we recommend using visual studio 2022/visual studio code IDE. 

# Usage

To run this project, 

* Install .NET SDK. Then using code editor/IDE, create a new console project and place all the C# codes inside your project folder. 
* Add/reference nuget package NeoCortexApi v1.1.4 to this project.
* Place numerical sequence CSV Files (datasets) under relevant folders respectively. All the folders should be inside the project folder. More details given below.

Our project is based on NeoCortex API. More details [NeoCortexAPI](https://github.com/ddobric/neocortexapi/blob/master/source/Documentation/gettingStarted.md).

# Details