using System;
using System.Globalization;
using System.Windows;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;

namespace PointsRounder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Model _model = new Model();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_RoundElements(object sender, RoutedEventArgs e)
        {
            string str = (string)null;
            try
            {
                if (this.chk_X.IsChecked.Value)
                    str += "X";
                if (this.chk_Y.IsChecked.Value)
                    str += "Y";
                if (this.chk_Z.IsChecked.Value)
                    str += "Z";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                string text = this.tb_CPartRound.Text;
                bool parsingResult = double
                    .TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double roundFactor);
                if (!parsingResult)
                {
                    throw new Exception($"Input string {text} has invalid format");
                }

                ModelObjectEnumerator selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector()
                    .GetSelectedObjects();
                double size = (double)selectedObjects.GetSize();
                while (selectedObjects.MoveNext())
                {
                    Type type = selectedObjects.Current.GetType();
                    string name = type.Name;
                    if (type.Name == "CustomPart")
                    {
                        Correct.RoundCPartPoints(selectedObjects.Current as CustomPart, roundFactor, str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "ContourPlate")
                    {
                        Correct.RoundCplatePoints(selectedObjects.Current as ContourPlate, roundFactor, str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "Beam")
                    {
                        Correct.RoundBeamPoints(selectedObjects.Current as Beam, roundFactor, str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "ControlLine")
                    {
                        Correct.RoundControlLinePoints(selectedObjects.Current as ControlLine, roundFactor, str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "PolyBeam")
                    {
                        Correct.RoundPolyBeamPoints(selectedObjects.Current as PolyBeam, roundFactor, str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "BooleanPart")
                    {
                        BooleanPart current = selectedObjects.Current as BooleanPart;
                        ContourPlate operativePart1 = current.OperativePart as ContourPlate;
                        try
                        {
                            Beam operativePart2 = current.OperativePart as Beam;
                            Correct.RoundBeamPoints(operativePart2, roundFactor, str);
                            current.OperativePart = (Tekla.Structures.Model.Part)operativePart2;
                        }
                        catch
                        {
                        }
                        try
                        {
                            Correct.RoundCplatePoints(operativePart1, roundFactor, str);
                            current.OperativePart = (Tekla.Structures.Model.Part)operativePart1;
                        }
                        catch
                        {
                        }
                        current.Modify();
                        this._model.CommitChanges();
                    }
                    if (type.Name == "RebarGroup")
                    {
                        RebarGroup current = selectedObjects.Current as RebarGroup;
                        Correct.RoundRebarGroupStartEndPoints(current, roundFactor, str);
                        current.Modify();
                        this._model.CommitChanges();
                    }
                    if (type.Name == "Component")
                    {
                        Correct.RoundComponentInputPoints(selectedObjects.Current as Component, roundFactor, str);
                        this._model.CommitChanges();
                    }
                }

                Operation.DisplayPrompt("Correction DONE!!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ModelObjectEnumerator selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();
            while (selectedObjects.MoveNext())
            {
                Type type = selectedObjects.Current.GetType();
                if (type.Name == "RebarGroup")
                    this._model.CommitChanges();
            }
        }
    }
}
