using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Handles file operations for training and inferring data in a system that processes CSV files.
/// </summary>
public class FileHandler
{
    // Default folders for training and inferring data.
    private readonly string defaultTrainingFolder;
    private readonly string defaultInferringFolder;

    /// <summary>
    /// List of validated training data files.
    /// </summary>
    public List<string> TrainingDataFiles { get; private set; } = new List<string>();

    /// <summary>
    /// List of validated inferring data files.
    /// </summary>
    public List<string> InferringDataFiles { get; private set; } = new List<string>();

    /// <summary>
    /// Initializes the FileHandler with default folders based on the project's base directory.
    /// </summary>
    public FileHandler()
    {
        string projectBaseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
        this.defaultTrainingFolder = Path.Combine(projectBaseDirectory, "TrainingData");
        this.defaultInferringFolder = Path.Combine(projectBaseDirectory, "InferringData");
    }

    /// <summary>
    /// Processes training and inferring data sources based on the provided arguments.
    /// </summary>
    /// <param name="trainingFile">Path to a specific training file.</param>
    /// <param name="inferringFile">Path to a specific inferring file.</param>
    /// <param name="trainingFolder">Path to a folder containing training files.</param>
    /// <param name="inferringFolder">Path to a folder containing inferring files.</param>
    /// <exception cref="Exception">Thrown when no valid training CSV files are found.</exception>
    public void ProcessFiles(string? trainingFile, string? inferringFile, string? trainingFolder, string? inferringFolder)
    {
        var allTrainingFiles = new List<string>();
        var allInferringFiles = new List<string>();

        Console.WriteLine("Gathering training data files...");
        if (!string.IsNullOrEmpty(trainingFile))
        {
            Console.WriteLine($"Adding training file: {trainingFile}");
            allTrainingFiles.Add(trainingFile);
        }

        if (!string.IsNullOrEmpty(trainingFolder) && Directory.Exists(trainingFolder))
        {
            Console.WriteLine($"Adding files from training folder: {trainingFolder}");
            allTrainingFiles.AddRange(Directory.GetFiles(trainingFolder));
        }

        if (string.IsNullOrEmpty(trainingFile) && string.IsNullOrEmpty(trainingFolder))
        {
            Console.WriteLine("Using default training folder...");
            if (Directory.Exists(defaultTrainingFolder))
            {
                allTrainingFiles.AddRange(Directory.GetFiles(defaultTrainingFolder));
            }
        }

        Console.WriteLine("Gathering inferring data files...");
        if (!string.IsNullOrEmpty(inferringFile))
        {
            Console.WriteLine($"Adding inferring file: {inferringFile}");
            allInferringFiles.Add(inferringFile);
        }

        if (!string.IsNullOrEmpty(inferringFolder) && Directory.Exists(inferringFolder))
        {
            Console.WriteLine($"Adding files from inferring folder: {inferringFolder}");
            allInferringFiles.AddRange(Directory.GetFiles(inferringFolder));
        }

        if (string.IsNullOrEmpty(inferringFile) && string.IsNullOrEmpty(inferringFolder))
        {
            Console.WriteLine("Using default inferring folder...");
            if (Directory.Exists(defaultInferringFolder))
            {
                allInferringFiles.AddRange(Directory.GetFiles(defaultInferringFolder));
            }
        }

        // Validate files and filter for CSVs
        Console.WriteLine("Validating and filtering training files...");
        TrainingDataFiles = ValidateAndFilterFiles(allTrainingFiles);

        Console.WriteLine("Validating and filtering inferring files...");
        InferringDataFiles = ValidateAndFilterFiles(allInferringFiles);

        if (TrainingDataFiles.Count == 0)
        {
            Console.WriteLine("Error: No valid training CSV files found.");
            throw new Exception("No valid training CSV files found. Program will terminate.");
        }

        Console.WriteLine("Training Data Files:");
        TrainingDataFiles.ForEach(file => Console.WriteLine($"  {file}"));

        Console.WriteLine("\nInferring Data Files:");
        InferringDataFiles.ForEach(file => Console.WriteLine($"  {file}"));
    }

    /// <summary>
    /// Validates a list of file paths and filters only valid CSV files.
    /// </summary>
    /// <param name="files">List of file paths to validate.</param>
    /// <returns>List of valid CSV file paths.</returns>
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

    /// <summary>
    /// Checks if a given file path has a .csv extension.
    /// </summary>
    /// <param name="filePath">File path to check.</param>
    /// <returns>True if the file is a CSV; otherwise, false.</returns>
    private bool IsCsv(string filePath)
    {
        return Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase);
    }
}