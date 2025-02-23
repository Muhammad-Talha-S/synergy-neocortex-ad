import argparse
import pandas as pd
import plotly.graph_objects as go

def parse_arguments():
    """Parse command-line arguments."""
    parser = argparse.ArgumentParser(description="Load CSV data and plot actual vs predicted values.")
    parser.add_argument("filename", type=str, default="sequence_1_predictions.csv", nargs="?", help="Path to the CSV file.")
    return parser.parse_args()

def load_and_clean_data(file_path):
    """Load CSV data, replace missing values, and convert to numeric."""
    dataframe = pd.read_csv(file_path)
    dataframe.replace("-", None, inplace=True)
    dataframe = dataframe.apply(pd.to_numeric, errors='coerce')
    return dataframe

def plot_actual_vs_predicted(dataframe):
    """Generate and display a Plotly figure comparing actual and predicted values with ±10% bounds."""
    actual_values = dataframe["Actual"]
    predicted_values = dataframe["Predicted"]
    x_axis = list(range(1, len(actual_values) + 1))
    
    # Calculate ±10% of predicted values
    upper_bound = predicted_values * 1.1
    lower_bound = predicted_values * 0.9
    
    fig = go.Figure()

    fig.add_trace(go.Scatter(
        x=x_axis, y=actual_values, mode='lines+markers', name='Actual Values',
        line=dict(color='#d62728'), connectgaps=True
    ))

    fig.add_trace(go.Scatter(
        x=x_axis, y=predicted_values, mode='lines+markers', name='Predicted Values',
        line=dict(color='#1f77b4'), connectgaps=True
    ))

    fig.add_trace(go.Scatter(
        x=x_axis, y=upper_bound, mode='lines', name='+10% Predicted',
        line=dict(color='#1f77b4', dash='dash'), connectgaps=True
    ))

    fig.add_trace(go.Scatter(
        x=x_axis, y=lower_bound, mode='lines', name='-10% Predicted',
        line=dict(color='#1f77b4', dash='dash'), connectgaps=True
    ))

    fig.update_layout(
        title='ML 24/25-03 Implement Anomaly Detection Sample; Team Synergy',
        xaxis_title='Sequence Position',
        yaxis_title='Value at Position'
    )
    fig.show()

def main():
    """Main function to load data and plot graph."""
    args = parse_arguments()
    sequence_df = load_and_clean_data(args.filename)
    plot_actual_vs_predicted(sequence_df)

if __name__ == "__main__":
    main()
