import pandas as pd
import plotly.graph_objects as go

df = pd.read_csv("sequence_1_predictions.csv")

df.replace("-", None, inplace=True)
df = df.apply(pd.to_numeric, errors='coerce')


actual_values = df["Actual"]
predicted_values = df["Predicted"]

x_axis = list(range(1, len(actual_values) + 1))

print(x_axis)
print(actual_values)
print(predicted_values)
