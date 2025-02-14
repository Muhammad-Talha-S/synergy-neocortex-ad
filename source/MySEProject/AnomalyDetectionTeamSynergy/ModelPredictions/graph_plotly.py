import pandas as pd
import plotly.graph_objects as go

sequence_df = pd.read_csv("sequence_1_predictions.csv")
sequence_df.replace("-", None, inplace=True)
sequence_df = sequence_df.apply(pd.to_numeric, errors='coerce')

actual_values = sequence_df["Actual"]
predicted_values = sequence_df["Predicted"]
x_axis = list(range(1, len(actual_values) + 1))


fig = go.Figure()

fig.add_trace(go.Scatter(x=x_axis, y=actual_values, mode='lines+markers', name='Actual Values'))
fig.add_trace(go.Scatter(x=x_axis, y=predicted_values, mode='lines+markers', name='Predicted Values'))

# Customize layout
fig.update_layout(title='Actual vs Predicted Values',
                  xaxis_title='Sequence Number',
                  yaxis_title='Values')

# Show the plot
fig.show()