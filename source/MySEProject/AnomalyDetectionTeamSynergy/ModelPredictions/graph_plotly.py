import argparse
import pandas as pd
import plotly.graph_objects as go

def parse_arguments():
    """Parse command-line arguments."""
    parser = argparse.ArgumentParser(description="Load CSV data and plot actual vs predicted values.")
    parser.add_argument("filename", type=str, default="sequence_3_predictions.csv", nargs="?", help="Path to the CSV file.")
    return parser.parse_args()

def load_and_clean_data(file_path):
    """Load CSV data, replace missing values, and convert to numeric."""
    dataframe = pd.read_csv(file_path)
    dataframe.replace("-", None, inplace=True)
    dataframe = dataframe.apply(pd.to_numeric, errors='coerce')
    return dataframe

def plot_actual_vs_predicted(dataframe):
    """Generate and display a Plotly figure comparing actual, predicted, and best-matched sequences with anomalies highlighted."""
    actual_values = dataframe["Inferring Sequence"]
    predicted_values = dataframe["Predicted Values"]
    best_matched_sequence = dataframe[" Best Matched Sequence"].dropna()
    x_axis = list(range(1, len(actual_values) + 1))

    # Calculate ±10% of predicted values
    upper_bound = predicted_values * 1.1
    lower_bound = predicted_values * 0.9

    # Identify anomalies
    anomalies = abs(actual_values - predicted_values) > (0.15 * actual_values)

    fig = go.Figure()

    # Plot inferring sequence values
    fig.add_trace(go.Scatter(
        x=x_axis, y=actual_values, mode='lines+markers', name='Inferring Sequence',
        line=dict(color='#d62728', width=2), marker=dict(size=6), connectgaps=True
    ))

    # Plot predicted values
    fig.add_trace(go.Scatter(
        x=x_axis, y=predicted_values, mode='lines+markers', name='Predicted Values',
        line=dict(color='#1f77b4', width=2, dash='dash'), marker=dict(size=6), connectgaps=True
    ))

    # Plot best-matched sequence
    fig.add_trace(go.Scatter(
        x=list(range(1, len(best_matched_sequence) + 1)), 
        y=best_matched_sequence, mode='lines+markers', name='Best Matched Sequence',
        line=dict(color='#2ca02c', width=2, dash='dot'), marker=dict(size=6), connectgaps=True
    ))

    # Plot ±10% bounds
    fig.add_trace(go.Scatter(
        x=x_axis, y=upper_bound, mode='lines', name='+10% Predicted',
        line=dict(color='#1f77b4', dash='dot'), connectgaps=True
    ))

    fig.add_trace(go.Scatter(
        x=x_axis, y=lower_bound, mode='lines', name='-10% Predicted',
        line=dict(color='#1f77b4', dash='dot'), connectgaps=True
    ))

    # Highlight anomalies
    fig.add_trace(go.Scatter(
        x=[x_axis[i] for i in range(len(x_axis)) if anomalies[i]],
        y=[actual_values[i] for i in range(len(actual_values)) if anomalies[i]],
        mode='markers', name='Anomalies',
        marker=dict(color='red', size=10, symbol='x')
    ))

    # Improve layout
    fig.update_layout(
        title='ML 24/25-03 Implement Anomaly Detection Sample; Team Synergy',
        xaxis_title='Sequence Position',
        yaxis_title='Value at Position',
    )
    
    fig.show()

def main():
    """Main function to load data and plot graph."""
    args = parse_arguments()
    sequence_df = load_and_clean_data(args.filename)
    plot_actual_vs_predicted(sequence_df)

if __name__ == "__main__":
    main()
