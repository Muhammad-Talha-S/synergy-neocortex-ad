import pandas as pd
import plotly.graph_objects as go

def load_and_clean_data(file_path):
    """Load CSV data, replace missing values, and convert to numeric."""
    df = pd.read_csv(file_path)
    df.replace("-", None, inplace=True)
    df = df.apply(pd.to_numeric, errors='coerce')
    return df

def plot_actual_vs_predicted(df):
    """Generate and display a Plotly figure comparing actual and predicted values."""
    actual_values = df["Actual"]
    predicted_values = df["Predicted"]
    x_axis = list(range(1, len(actual_values) + 1))
    
    fig = go.Figure()
    fig.add_trace(go.Scatter(x=x_axis, y=actual_values, mode='lines+markers', name='Actual Values'))
    fig.add_trace(go.Scatter(x=x_axis, y=predicted_values, mode='lines+markers', name='Predicted Values', connectgaps=True))
    
    fig.update_layout(title='Actual vs Predicted Values', xaxis_title='Sequence Number', yaxis_title='Values')
    fig.show()

def main():
    """Main function to load data and plot graph."""
    file_path = "sequence_1_predictions.csv"
    sequence_df = load_and_clean_data(file_path)
    plot_actual_vs_predicted(sequence_df)

if __name__ == "__main__":
    main()
