namespace Airports_Logic.Attributes
{
    using System;

    /// <summary>
    /// This is an attribute on the model classes. Binds the columns with the properties of a class.
    /// </summary>
    public class Column : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        /// <param name="name">Name of the column in the file to be binded to the following property.</param>
        public Column(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the Name of the column in the file to be binded to the following property.
        /// </summary>
        public string Name { get; set; }
    }
}
