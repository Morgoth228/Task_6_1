﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task_6_1;
using MusicalInstrumentClassLibrary;
using System.Configuration.Assemblies;


namespace Task_6_1
{
    public partial class Form1 : Form
    {
        private List<string> classList;

        public Form1()
        {
            InitializeComponent();

            classList = ReflectionExample.GetImplementingClasses("MusicalInstrumentClassLibrary.dll", typeof(IMusicalInstrument));

            foreach (string className in classList)
            {
                comboBoxClasses.Items.Add(className);
            }
        }

        

        private void buttonExecute_Click_1(object sender, EventArgs e)
        {
            string className = comboBoxClasses.SelectedItem.ToString();
            Type type = Assembly.LoadFrom("MusicalInstrumentClassLibrary.dll").GetType("MusicalInstrumentClassLibrary." + className);


            object obj = Activator.CreateInstance(type);

            MethodInfo method = type.GetMethod(comboBoxMethods.SelectedItem.ToString());

            ParameterInfo[] parameters = method.GetParameters();

            string message = "";

            if (parameters.Length == 0)
            {
                message = method.Invoke(obj, null).ToString();
            }
            else
            {
                List<object> args = new List<object>();

                foreach (ParameterInfo parameter in parameters)
                {
                    string input = Interaction.InputBox($"Введите значение параметра {parameter.Name} для функции {method.Name}", "Значение параметра");
                    object arg = Convert.ChangeType(input, parameter.ParameterType);
                    args.Add(arg);
                }
                message = method.Invoke(obj, args.ToArray()).ToString();

            }
            MessageBox.Show(message ?? "Результата вывода нет");
        }

        private void comboBoxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            string className = comboBoxClasses.SelectedItem.ToString();
            Type type = Assembly.LoadFrom("MusicalInstrumentClassLibrary.dll").GetType("MusicalInstrumentClassLibrary." + className);
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            comboBoxMethods.Items.Clear();
            foreach (MethodInfo method in methods)
            {
                comboBoxMethods.Items.Add(method.Name);
            }
        }
    }
}
