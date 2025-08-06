using System;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;

namespace PointsRounder
{
    public class Correct //класс корректировки элементов с плохими координатами
    {
        public static void RoundBeamCoord(Beam beam, double rValue) //метод округления координат начала и конца балки до указанной пользователем величины
        {
            Point sPointNew = new Point(); //создаём новый объект типа точка для новой стартовой точки балки
            Point ePointNew = new Point(); //создаём новый объект типа точка для новой конечной точки балки

            Point sPointOld = beam.StartPoint; //получаем старую стартовую точку
            Point ePointOld = beam.EndPoint; //получаем старую конечную точку

            sPointNew.X = Math.Round(sPointOld.X / rValue, 0) * rValue; //округляем координату начала (X)
            sPointNew.Y = Math.Round(sPointOld.Y / rValue, 0) * rValue; //округляем координату начала (Y)
            sPointNew.Z = Math.Round(sPointOld.Z / rValue, 0) * rValue; //округляем координату начала (Z)

            ePointNew.X = Math.Round(ePointOld.X / rValue, 0) * rValue; //округляем координату конца (X)
            ePointNew.Y = Math.Round(ePointOld.Y / rValue, 0) * rValue; //округляем координату конца (Y)
            ePointNew.Z = Math.Round(ePointOld.Z / rValue, 0) * rValue; //округляем координату конца (Z)

            beam.StartPoint.X = beam.StartPoint.X + 100; //определяем фиктивную координату начала (X) + 100мм
            beam.StartPoint.Y = beam.StartPoint.Y + 100; //определяем фиктивную координату начала (Y) + 100мм
            beam.StartPoint.Z = beam.StartPoint.Z + 100; //определяем фиктивную координату начала (Z) + 100мм

            beam.EndPoint.X = beam.EndPoint.X + 100; //определяем фиктивную координату конца (X) + 100мм
            beam.EndPoint.Y = beam.EndPoint.Y + 100; //определяем фиктивную координату конца (Y) + 100мм
            beam.EndPoint.Z = beam.EndPoint.Z + 100; //определяем фиктивную координату конца (Z) + 100мм

            beam.Modify(); //обновляем балку до фиктивных координат

            beam.StartPoint = sPointNew; //назначаем обновлённую округлённую точку начала
            beam.EndPoint = ePointNew; //назначаем обновлённую округлённую точку конца

            beam.Modify(); //обновляем балку

            StatusBarMessage(beam); //выводим значение об обработке в консоль
        }

        public static void RoundCPartPoints(CustomPart cmp, double rValue, string coord)
        {
            Point pt1 = new Point(); //создаём точки для получения точек вставки
            Point pt2 = new Point(); //создаём точки для получения точек вставки


            //округляем полученные точки вставки
            Point pt1round = RoundPoint(pt1, rValue, coord);
            Point pt2round = RoundPoint(pt2, rValue, coord);

            // перемещаем объект на 100 мм в сторону, чтобы потом нормально вернуть назад
            _ = cmp.SetInputPositions(
                new Point(pt1.X + 100, pt1.Y + 100, pt1.Z + 100),
                new Point(pt2.X + 100, pt2.Y + 100, pt2.Z + 100));
            cmp.Modify();

            // перемещаем объект на правильные округлённые точки
            _ = cmp.SetInputPositions(pt1round, pt2round);
            cmp.Modify();
            //вызываем методо сообщения о выполнении команды
            StatusBarMessage(cmp);
        }

        //метод округления координат контурных пластин
        public static void RoundCplatePoints(ContourPlate cplate, double rValue, string coord)
        {
            foreach (ContourPoint cPoint in cplate.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                cPoint.X += 100;//присваиваем значение координаты
                cPoint.Y += 100;//присваиваем значение координаты
                cPoint.Z += 100;//присваиваем значение координаты
            }
            cplate.Modify();//модифицируем контурную пластину

            foreach (ContourPoint cPoint in cplate.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                //получаем значение координаты
                cPoint.X -= 100;
                cPoint.Y -= 100;
                cPoint.Z -= 100;

                //округляем текущую точку
                ContourPoint cpRound = RoundContourPoint(cPoint, rValue, coord);

                //назначаем координаты округлённые
                cPoint.X = cpRound.X;
                cPoint.Y = cpRound.Y;
                cPoint.Z = cpRound.Z;

            }
            cplate.Modify();//модифицируем контурную пластину

            //вызываем методо сообщения о выполнении команды
            StatusBarMessage(cplate);
        }

        public static Point RoundPoint(Point pt, double rValue, string coordinates)
        {
            char[] charArray = coordinates.ToCharArray(); //дробим строку по каким координатам округляем

            Point ptres = pt;
            //проходимся по массиву букв и сравниваем какие координаты округляем
            //округляем только нужные координаты
            foreach (char k in charArray)
            {
                switch (k)
                {
                    case 'X': // округляем координату по X
                        ptres.X = RoundValue(pt.X, rValue);
                        break;
                    case 'Y': // округляем координату по Y
                        ptres.Y = RoundValue(pt.Y, rValue);
                        break;
                    case 'Z': // округляем координату по Z
                        ptres.Z = RoundValue(pt.Z, rValue);
                        break;
                }
            }

            return ptres;
        }

        // метод для округления координат контурной точки по входному значению и строке координат округления
        public static ContourPoint RoundContourPoint(ContourPoint pt, double rValue, string coordinates)
        {
            char[] charArray = coordinates.ToCharArray(); //дробим строку по каким координатам округляем

            ContourPoint ptres = new ContourPoint
            {
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            foreach (char k in charArray)
            {
                switch (k)
                {
                    case 'X': // округляем координату по X
                        ptres.X = RoundValue(pt.X, rValue);
                        break;
                    case 'Y': // округляем координату по Y
                        ptres.Y = RoundValue(pt.Y, rValue);
                        break;
                    case 'Z': // округляем координату по Z
                        ptres.Z = RoundValue(pt.Z, rValue);
                        break;
                }
            }

            return ptres;
        }

        //метод округления точек контроллайн
        public static void RoundControlLinePoints(ControlLine cline, double rValue, string coord)
        {
            //создаём точки для получения из контроллайн
            Point p1 = cline.Line.Point1;
            Point p2 = cline.Line.Point2;

            //создаём фиктивные точки
            Point p1f = new Point();
            Point p2f = new Point();

            //определяем координаты для фиктивных точек (т.1)
            p1f.X = p1.X + 100;
            p1f.Y = p1.Y + 100;
            p1f.Z = p1.Z + 100;

            //определяем координаты для фиктивных точек (т.1)
            p2f.X = p2.X + 100;
            p2f.Y = p2.Y + 100;
            p2f.Z = p2.Z + 100;

            //изменяем controlline по фиктивным точкам 
            cline.Line.Point1 = p1f;
            cline.Line.Point2 = p2f;
            cline.Modify();

            //округляем первоначальные координаты
            p1 = RoundPoint(p1, rValue, coord);
            p2 = RoundPoint(p2, rValue, coord);

            //назначаем координаты изначальной controlline
            cline.Line.Point1 = p1;
            cline.Line.Point2 = p2;

            //изменяем controlline
            cline.Modify();

            //вызываем метод о сообщения о выполнении команды
            StatusBarMessage(cline);
        }

        //метод округления точек арматурной группы
        public static void RoundRebarGroupStartEndPoints(RebarGroup rb, double rValue, string coord)
        {
            #region обрабатывает точки диапазона распределения
            //создаём точки для получения данных о начале и конце диапазона распространения арматурной группы
            Point p1 = rb.StartPoint;
            Point p2 = rb.EndPoint;

            //создаём фиктивные точки
            Point p1f = new Point();
            Point p2f = new Point();

            //определяем координаты для фиктивных точек (т.1)
            p1f.X = p1.X + 100;
            p1f.Y = p1.Y + 100;
            p1f.Z = p1.Z + 100;

            //определяем координаты для фиктивных точек (т.1)
            p2f.X = p2.X + 100;
            p2f.Y = p2.Y + 100;
            p2f.Z = p2.Z + 100;

            //изменяем арматурную группу по фиктивным точкам 
            rb.StartPoint = p1f;
            rb.EndPoint = p2f;
            rb.Modify();

            //округляем первоначальные координаты
            p1 = RoundPoint(p1, rValue, coord);
            p2 = RoundPoint(p2, rValue, coord);

            //назначаем координаты изначальной арматурной группе
            rb.StartPoint = p1;
            rb.EndPoint = p2;
            #endregion

            #region Обрабатываем точки формы арматурного стержня
            Polygon plgf = new Polygon(); //полигон для фиктивных точек арматурного стержня
            Polygon plg = rb.Polygons[0] as Polygon; //получаем полигон точек из арматурного стержня

            //в цикле проходимся по точкам и получаем полигон фиктивных точек
            foreach (Point n in plg.Points)
            {
                Point pt = new Point(n.X + 100, n.Y + 100, n.Z + 100);
                plgf.Points.Add(pt);
            }

            rb.Polygons.Clear();//чистим полигон точек внутри арматурного стержня
            rb.Polygons.Add(plgf);//добавляем полигон фиктивных точек в арматурную группу
            rb.Modify();//обновляем арматурную группу

            plg.Points.Clear();//чистим исходный полигон точек

            //в цикле проходимя по точкам фиктивного полигона, округляем их и возвращаем в прежнее положение
            foreach (Point n in plgf.Points)
            {
                Point pt = new Point(n.X - 100, n.Y - 100, n.Z - 100);
                pt = RoundPoint(pt, rValue, coord);
                plg.Points.Add(pt);
            }

            rb.Polygons.Clear();//чистим исходный полигон
            rb.Polygons.Add(plg); //добавляем полигон исходных точек
            #endregion

            //изменяем 
            rb.Modify();

            //вызываем метод о сообщения о выполнении команды
            StatusBarMessage(rb);
        }

        //метод округления точек начальных и конечных простых балок
        public static void RoundBeamPoints(Beam beam, double rValue, string coord)
        {
            Point p1 = beam.StartPoint;
            Point p2 = beam.EndPoint;

            beam.StartPoint = new Point(p1.X + 100, p1.Y + 100, p1.Z + 100);
            beam.EndPoint = new Point(p2.X + 100, p2.Y + 100, p2.Z + 100);

            p1 = RoundPoint(p1, rValue, coord);
            p2 = RoundPoint(p2, rValue, coord);

            beam.StartPoint = p1;
            beam.EndPoint = p2;

            beam.Modify();

            StatusBarMessage(beam);
        }

        //метод округления точек для polybeam
        public static void RoundPolyBeamPoints(PolyBeam pBeam, double rValue, string coord)
        {
            foreach (ContourPoint pPoint in pBeam.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                pPoint.X += 100;//присваиваем значение координаты
                pPoint.Y += 100;//присваиваем значение координаты
                pPoint.Z += 100;//присваиваем значение координаты
            }
            pBeam.Modify();//модифицируем контурную пластину

            foreach (ContourPoint pPoint in pBeam.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                //получаем значение координаты
                pPoint.X -= 100;
                pPoint.Y -= 100;
                pPoint.Z -= 100;

                //округляем текущую точку
                ContourPoint cpRound = RoundContourPoint(pPoint, rValue, coord);

                //назначаем координаты округлённые
                pPoint.X = cpRound.X;
                pPoint.Y = cpRound.Y;
                pPoint.Z = cpRound.Z;

            }
            pBeam.Modify();//модифицируем полибалку

            //вызываем методо сообщения о выполнении команды
            StatusBarMessage(pBeam);
        }

        public static void RoundComponentInputPoints(Component cmp, double rValue, string coord)
        {
            var inputObjects = cmp.GetComponentInput(); //получаем входные объекты плагина
            ComponentInput cmpI = new ComponentInput(); //создаём новый объект входных данных

            foreach (var k in inputObjects)
            {
                InputItem im = k as InputItem; //распаковываем элемент как inputitem
                //если объект точка
                try
                {
                    Point pti = im.GetData() as Point; //распаковываем элемент, как точку
                    pti = RoundPoint(pti, rValue, coord); //округляем точку
                    cmpI.AddOneInputPosition(pti);//добавляем точку в вводные данные плагина
                }
                //если объект не точка, то просто добавляем обратно к списку объектов плагина
                catch
                {
                    ModelObject moc = im.GetData() as ModelObject;
                    cmpI.AddInputObject(moc);
                }

            }
            //переназначаем входные данные
            cmp.SetComponentInput(cmpI);
            cmp.Modify();//изменяем плагин
            StatusBarMessage(cmp);//выводим сообщение
        }

        public static void StatusBarMessage(ModelObject mo)
        {
            string type = mo.GetType().Name; //получаем имя элемента
            string guid = mo.Identifier.GUID.ToString(); //получаем guid
            string resultString = type + " " + guid + "- DONE!!!"; // клеем выводную строку
            Operation.DisplayPrompt(resultString); //выводим в консоль
        }

        private static double RoundValue(double value, double rValue)
        {
            double result = Math.Round(value / rValue, 0) * rValue;
            return result;
        }
    }
}