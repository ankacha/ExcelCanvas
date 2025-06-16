// Models/ExcelFunction.cs

using System.Windows.Input;

namespace CanvasTest.Models //Tells the compiler this is part of the Models namespace
{   // Add .ViewModels here

    public class ExcelFunction
    {
        public string Name { get; set; } = string.Empty;//expose the function name to the UI
        public string Description { get; set; } = string.Empty;// expose the function description to the UI
        public string Category { get; set; } = string.Empty;// expose the function category to the UI
        public string IconPath { get; set; } = string.Empty;// expose the function icon path to the UI

        public string FormulaPattern { get; set; } = string.Empty; // expose the function formula pattern to the UI
        public List<InputDefinition> Inputs { get; set; } = new List<InputDefinition>();

        public class InputDefinition
        {
            public string Name { get; set; } = string.Empty;
            public string Label { get; set; } = string.Empty;
            public InputType Type { get; set; } = InputType.Value;
        }

        public enum InputType
        {
            Range, //A1:A10, B2:B20, etc.
            Value, // 5, 10.5, etc.
            Formula, // =SUM(A1:A10), =AVERAGE(B2:B20), etc.
            Text // "Hello", "World", etc.
        }

        public static List<ExcelFunction> GetAvailableFunctions() //
        {
            return new List<ExcelFunction> // This returns a generated list of available Excel functions which are 
                {                            //represented by an instance of ExcelFunction so that they can be used in the UI
                    {
                    new ExcelFunction //creates a new instance of ExcelFunction
                        {
                            Name = "SUM", //sets the name of the function
                            Description = "Adds all values together", // sets the description of the function
                            Category = "Math", // sets the category of the function
                            IconPath = "M12,2 L12,22 M2,12 L22,12",  // Plus sign icon
                            FormulaPattern = "SUM({0})", // sets the formula pattern for the function, which is used to generate the formula in the UI
                            Inputs = new List<InputDefinition> // defines the inputs for the function
                                {
                                    new InputDefinition { Name = "Range", Label = "Range", Type = InputType.Range } // defines a range input for the function
                                }
                        }
                    },

                    new ExcelFunction
                    {
                        Name = "AVERAGE",
                        Description = "Calculates the mean value",
                        Category = "Statistical",
                        IconPath = "M3,12 L21,12 M7,6 L7,18 M12,6 L12,18 M17,6 L17,18",  // Bars
                        FormulaPattern = "AVERAGE({0})",
                        Inputs = new List<InputDefinition>
                            {
                                new InputDefinition { Name = "range", Label = "Values", Type = InputType.Range }
                            
                            }
                    },

                    new ExcelFunction
                    {
                        Name = "MIN",
                        Description = "Returns the smallest value",
                        Category = "Statistical",
                        IconPath = "M3,7 L9,13 L13,9 L21,17 M21,17 L15,17 M21,17 L21,11",  // Downward trend
                        //Execute = values => values.Min()

                    },

                    new ExcelFunction
                    {
                        Name = "MAX",
                        Description = "Returns the largest value",
                        Category = "Statistical",
                        IconPath = "M3,17 L9,11 L13,15 L21,7 M21,7 L15,7 M21,7 L21,13",  // Upward trend
                        //Execute = values => values.Max()
                    },

                    new ExcelFunction
                    {
                        Name = "COUNT",
                        Description = "Counts the number of values",
                        Category = "Statistical",
                        IconPath = "M7,2 L7,8 M4,5 L10,5 M17,2 L17,8 M14,5 L20,5 M7,11 L7,17 M4,14 L10,14 M17,11 L17,17 M14,14 L20,14",  // Hash/Count symbol
                        //Execute = values => (double)values.Count()

                    },

                    new ExcelFunction
                    {
                        Name = "PRODUCT",
                        Description = "Multiplies all values together",
                        Category = "Math",
                        IconPath = "M6,6 L18,18 M18,6 L6,18",  // X (multiply)
                        //Execute = values => values.Aggregate(1.0, (a, b) => a * b)
                    },

                    new ExcelFunction
                    {
                        Name = "POWER",
                        Description = "Raises first value to power of second",
                        Category = "Math",
                        IconPath = "M3,14 L9,4 M9,4 L9,8 M9,4 L5,4 M14,4 L18,4 L20,6 L20,8 L18,10 L14,10",  // Superscript
                        //Execute = values => values.Count() >= 2 ? Math.Pow(values.First(), values.Skip(1).First()) : 0
                    },

                    new ExcelFunction
                    {
                        Name = "SQRT",
                        Description = "Calculates square root",
                        Category = "Math",
                        IconPath = "M5,10 L8,19 L11,3 L15,22",  // Root symbol
                        //Execute = values => Math.Sqrt(values.FirstOrDefault())
                    },

                    new ExcelFunction
                    {
                        Name = "ABS",
                        Description = "Returns absolute value",
                        Category = "Math",
                        IconPath = "M8,4 L8,20 M16,4 L16,20 M3,12 L21,12",  // |x|
                        //Execute = values => Math.Abs(values.FirstOrDefault())

                    },


                    // Rounding Functions

                    new ExcelFunction
                    {
                        Name = "ROUND",
                        Description = "Rounds to nearest integer",
                        Category = "Math",
                        IconPath = "M12,3 C16.97,3 21,7.03 21,12 C21,16.97 16.97,21 12,21 C7.03,21 3,16.97 3,12",  // Circle
                        //Execute = values => Math.Round(values.FirstOrDefault())
                    },
                    new ExcelFunction
                    {
                        Name = "CEILING",
                        Description = "Rounds up to nearest integer",
                        Category = "Math",
                        IconPath = "M4,18 L20,18 M12,18 L12,6 M12,6 L8,10 M12,6 L16,10",  // Up arrow
                        //Execute = values => Math.Ceiling(values.FirstOrDefault())
                    },
                    new ExcelFunction
                    {
                       Name = "FLOOR",
                       Description = "Rounds down to nearest integer",
                        Category = "Math",
                        IconPath = "M4,6 L20,6 M12,6 L12,18 M12,18 L8,14 M12,18 L16,14",  // Down arrow
                        //Execute = values => Math.Floor(values.FirstOrDefault())

                    },


                    // Statistical Functions

                    new ExcelFunction
                    {
                        Name = "MEDIAN",
                        Description = "Returns the middle value",
                        Category = "Statistical",
                        IconPath = "M4,6 L4,18 M12,3 L12,21 M20,6 L20,18",  // Middle line emphasized

                    },

                    new ExcelFunction
                    {
                        Name = "MODE",
                        Description = "Returns most frequent value",
                        Category = "Statistical",
                        IconPath = "M4,18 L4,12 M8,18 L8,8 M12,18 L12,4 M16,18 L16,8 M20,18 L20,12",  // Bar chart with peak


                    },

                    new ExcelFunction
                    {
                        Name = "STDEV",
                        Description = "Calculates standard deviation",
                        Category = "Statistical",
                        IconPath = "M12,2 C13.1,2 14,2.9 14,4 C14,5.1 13.1,6 12,6 C10.9,6 10,5.1 10,4 C10,2.9 10.9,2 12,2 M12,8 L12,16 M8,12 L16,12 M8,16 L16,16",  // Sigma-like

                    },

                    new ExcelFunction

                    {
                        Name = "VAR",
                        Description = "Calculates variance",
                        Category = "Statistical",
                        IconPath = "M5,3 L5,21 M5,12 L19,12 M19,8 L19,16",  // Spread indicator

                    },

                    // Logical Functions
                    new ExcelFunction
                    {
                        Name = "IF",
                        Description = "Returns value based on condition",
                        Category = "Logical",
                        IconPath = "M12,2 L12,9 M12,9 L9,6 M12,9 L15,6 M12,22 L12,15 M12,15 L9,18 M12,15 L15,18",  // Branching
                        FormulaPattern = "IF({0},{1},{2})",
                        Inputs = new List<InputDefinition>
                        {
                            new InputDefinition { Name = "condition", Label = "Test", Type = InputType.Formula },
                            new InputDefinition { Name = "valueIfTrue", Label = "If True", Type = InputType.Value },
                            new InputDefinition { Name = "valueIfFalse", Label = "If False", Type = InputType.Value }
                        }
                    },

                    new ExcelFunction
                    {
                        Name = "AND",
                        Description = "Returns 1 if all values are non-zero",
                        Category = "Logical",
                        IconPath = "M8,4 C8,8 8,8 12,12 M16,4 C16,8 16,8 12,12 M12,12 L12,20",  // Logic gate AND

                    },

                    new ExcelFunction

                    {
                        Name = "OR",
                        Description = "Returns 1 if any value is non-zero",
                        Category = "Logical",
                        IconPath = "M8,4 C8,10 8,10 12,12 M16,4 C16,10 16,10 12,12 M12,12 L12,20 M8,4 Q12,6 16,4",  // Logic gate OR
                        //Execute = values => values.Any(v => v != 0) ? 1.0 : 0.0
                    },
                    new ExcelFunction
                    {
                        Name = "NOT",
                        Description = "Returns opposite (1 becomes 0, 0 becomes 1)",
                        Category = "Logical",
                        IconPath = "M12,8 L12,12 M12,12 C14,12 16,14 16,16 C16,18 14,20 12,20 C10,20 8,18 8,16 C8,14 10,12 12,12",  // NOT gate
                        /////*Execute*/ = values => values.FirstOrDefault() == 0 ? 1.0 : 0.0
                    },

                   // Text/Conversion Functions
                    new ExcelFunction
                    {
                        Name = "LEN",
                        Description = "Returns length of text representation",
                        Category = "Text",
                        IconPath = "M3,12 L7,12 M17,12 L21,12 M7,12 L7,8 M7,12 L7,16 M17,12 L17,8 M17,12 L17,16",  // Measure
                        //Execute = values => (double)values.FirstOrDefault().ToString().Length
                    },
                    new ExcelFunction
                    {
                        Name = "VALUE",
                        Description = "Converts text to number",
                        Category = "Text",
                        IconPath = "M7,4 L7,7 M7,7 L4,7 M17,4 L17,7 L14,7 L14,10 L17,10 L17,13 L14,13 M7,17 L4,20 M7,17 L10,20",  // 123
                        //Execute = values => values.FirstOrDefault()

                    },


                    // Financial Functions

                    new ExcelFunction
                   {
                        Name = "PMT",
                        Description = "Calculates loan payment",
                        Category = "Financial",
                        IconPath = "M12,2 C6.48,2 2,6.48 2,12 C2,17.52 6.48,22 12,22 C17.52,22 22,17.52 22,12 M12,6 L12,18 M9,9 L15,9",  // Dollar/Currency
                        //Execute = values => {
                        //    if (values.Count() < 3) return 0;
                        //    var rate = values.ElementAt(0) / 12 / 100; // Annual rate to monthly
                        //    var nper = values.ElementAt(1);
                        //    var pv = values.ElementAt(2);
                        //    if (rate == 0) return -pv / nper;
                        //    return -rate * pv / (1 - Math.Pow(1 + rate, -nper));
                        //}
                    },

                    new ExcelFunction
                    {
                        Name = "FV",
                        Description = "Calculates future value",
                        Category = "Financial",
                        IconPath = "M3,3 L3,18 L18,18 M6,15 L9,10 L12,13 L18,5",  // Growth chart

                    },


                    // Date Functions (using days since epoch)

                    new ExcelFunction
                    {
                        Name = "TODAY",
                        Description = "Returns today as number",
                        Category = "Date",
                        IconPath = "M19,3 L5,3 C3.89,3 3,3.9 3,5 L3,19 C3,20.1 3.89,21 5,21 L19,21 C20.1,21 21,20.1 21,19 L21,5 C21,3.9 20.1,3 19,3 M19,5 L19,7 L5,7 L5,5 L19,5 M5,19 L5,9 L19,9 L19,19 L5,19",  // Calendar
                    },

                    new ExcelFunction
                    {
                        Name = "DAYS",
                        Description = "Days between two dates",
                        Category = "Date",
                        IconPath = "M9,10 L9,15 L15,15 L15,10 L9,10 M7,2 L7,5 M17,2 L17,5 M3,7 L21,7",  // Calendar range

                    },
                   new ExcelFunction
                    {
                        Name = "CONVERT",
                        Description = "Converts units (simplified)",
                        Category = "Engineering",
                        IconPath = "M16,12 L20,8 M20,8 L16,8 M20,8 L20,12 M8,12 L4,16 M4,16 L8,16 M4,16 L4,12",  // Conversion arrows
                    },

                   //Lookup functions
                    new ExcelFunction
                    {
                        Name = "VLOOKUP",
                        Description = "Looks up a value in a table",
                        Category = "Lookup",
                        IconPath = "M3,3 L3,21 M3,12 L21,12",
                        FormulaPattern = "VLOOKUP({0},{1},{2},{3})",
                        Inputs = new List<InputDefinition>
                            {
                                new InputDefinition { Name = "lookupValue", Label = "Lookup", Type = InputType.Value },
                                new InputDefinition { Name = "tableArray", Label = "Table", Type = InputType.Range },
                                new InputDefinition { Name = "colIndex", Label = "Column", Type = InputType.Value },
                                new InputDefinition { Name = "rangeLookup", Label = "Exact", Type = InputType.Value }
                            }

                    }
                };
        }
    }
}