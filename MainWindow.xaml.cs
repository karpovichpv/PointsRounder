using Point = Tekla.Structures.Geometry3d.Point;

namespace PointsRounder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model = new Model();

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
                int num = (int)MessageBox.Show(ex.Message);
            }
            try
            {
                ModelObjectEnumerator selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();
                double size = (double)selectedObjects.GetSize();
                while (selectedObjects.MoveNext())
                {
                    Type type = selectedObjects.Current.GetType();
                    string name = type.Name;
                    if (type.Name == "CustomPart")
                    {
                        Correct.roundCPartPoints(selectedObjects.Current as CustomPart, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "ContourPlate")
                    {
                        Correct.roundCplatePoints(selectedObjects.Current as ContourPlate, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "Beam")
                    {
                        Correct.roundBeamPoints(selectedObjects.Current as Beam, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "ControlLine")
                    {
                        Correct.roundControlLinePoints(selectedObjects.Current as ControlLine, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "PolyBeam")
                    {
                        Correct.roundPolyBeamPoints(selectedObjects.Current as PolyBeam, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                        this._model.CommitChanges();
                    }
                    if (type.Name == "BooleanPart")
                    {
                        BooleanPart current = selectedObjects.Current as BooleanPart;
                        ContourPlate operativePart1 = current.OperativePart as ContourPlate;
                        try
                        {
                            Beam operativePart2 = current.OperativePart as Beam;
                            Correct.roundBeamPoints(operativePart2, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                            current.OperativePart = (Tekla.Structures.Model.Part)operativePart2;
                        }
                        catch
                        {
                        }
                        try
                        {
                            Correct.roundCplatePoints(operativePart1, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
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
                        Correct.roundRebarGroupStartEndPoints(current, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                        current.Modify();
                        this._model.CommitChanges();
                    }
                    if (type.Name == "Component")
                    {
                        Correct.roundComponentInputPoints(selectedObjects.Current as Tekla.Structures.Model.Component, (int)Convert.ToInt16(this.tb_CPartRound.Text), str);
                        this._model.CommitChanges();
                    }
                }
                Operation.DisplayPrompt("Correction DONE!!!");
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_Offset(object sender, RoutedEventArgs e)
        {
            try
            {
                ModelObjectEnumerator selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();
                while (selectedObjects.MoveNext())
                {
                    if (selectedObjects.Current.GetType().Name == "ContourPlate")
                    {
                        OffsetElements.OffsetContourPlate(selectedObjects.Current as ContourPlate, -1.0 * Convert.ToDouble(this.tbOffsetValue.Text));
                        this._model.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ModelObjectEnumerator selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();
            double size = (double)selectedObjects.GetSize();
            while (selectedObjects.MoveNext())
            {
                Type type = selectedObjects.Current.GetType();
                string name = type.Name;
                if (type.Name == "RebarGroup")
                    this._model.CommitChanges();
            }
        }

        private void Button_btDrawOpenningSymbol(object sender, RoutedEventArgs e)
        {
            Tekla.Structures.Drawing.UI.Picker picker = new DrawingHandler().GetPicker();
            Tuple<Point, Tekla.Structures.Drawing.ViewBase> tuple1 = picker.PickPoint("Pick Point 1");
            Tuple<Point, Tekla.Structures.Drawing.ViewBase> tuple2 = picker.PickPoint("Pick Point 1");
            Point point1 = new Point();
            Point point2 = new Point();
            Point point3;
            Point point4;
            if (tuple1.Item1.Y < tuple2.Item1.Y)
            {
                if (tuple1.Item1.X < tuple2.Item1.X)
                {
                    point3 = tuple1.Item1;
                    point4 = tuple2.Item1;
                }
                else
                {
                    point3 = new Point(tuple2.Item1.X, tuple1.Item1.Y);
                    point4 = new Point(tuple1.Item1.X, tuple2.Item1.Y);
                }
            }
            else if (tuple1.Item1.X < tuple2.Item1.X)
            {
                point3 = new Point(tuple1.Item1.X, tuple2.Item1.Y);
                point4 = new Point(tuple2.Item1.X, tuple1.Item1.Y);
            }
            else
            {
                point3 = tuple2.Item1;
                point4 = tuple1.Item1;
            }
            double num1 = 5.0;
            double num2 = point4.Y - point3.Y;
            double num3 = point4.X - point3.X;
            double num4 = Math.Min(num2 / num1, num3 / num1);
            Point point5 = new Point(point3.X + num4, point4.Y - num4);
            Point point6 = new Point(point3.X, point4.Y);
            PointList pointList = new PointList();
            pointList.Add(point3);
            pointList.Add(point6);
            pointList.Add(point4);
            pointList.Add(point5);
            Tekla.Structures.Drawing.Polygon.PolygonAttributes polygonAttributes = new Tekla.Structures.Drawing.Polygon.PolygonAttributes();
            polygonAttributes.Hatch.Name = "hardware_SOLID";
            new Tekla.Structures.Drawing.Polygon(tuple1.Item2, pointList)
            {
                Attributes = polygonAttributes
            }.Insert();
        }
    }
}
