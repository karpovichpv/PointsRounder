namespace PointsRounder
{
    public class Correct //класс корректировки элементов с плохими координатами
    {
        public static void moveToZbeam(Beam beam, double z) //метод перемещения балки с плохими точками на координату Z
        {
            Point sPointNew = new Point(); //создаём новый объект типа точка для новой стартовой точки балки
            Point ePointNew = new Point(); //создаём новый объект типа точка для новой конечной точки балки

            Point sPointOld = new Point(); //создаём новый объект типа точка для старой стартовой точки балки
            Point ePointOld = new Point(); //создаём новый объект типа точка для конечной стартовой точки балки

            sPointOld = beam.StartPoint; //назначаем старую стартовую точку
            ePointOld = beam.EndPoint; //назначаем новую стартовую точку

            sPointNew.X = sPointOld.X; //назначаем координату X для стартовой точки
            sPointNew.Y = sPointOld.Y; //назначаем координату Y для стартовой точки
            sPointNew.Z = z + 100; //назначаем координату Z для стартовой точки (+100мм)

            ePointNew.X = ePointOld.X; //назначаем координату X для конечной точки
            ePointNew.Y = ePointOld.Y; //назначаем координату Y для конечной точки
            ePointNew.Z = z + 100; //назначаем координату Z для конечной точки (+100мм)

            beam.StartPoint = sPointNew; //назначаем стартовую точку для балки (обновлённую)
            beam.EndPoint = ePointNew; //назначаем конечную точку для балки (обновлённую)

            beam.Modify(); //обновляем балку

            sPointNew.Z = z; //назначаем координату Z для стартовой точки
            ePointNew.Z = z; //назначаем координату Z для конечной точки

            beam.StartPoint = sPointNew; //назначаем стартовую точку для балки (обновлённую)
            beam.EndPoint = ePointNew; //назначаем конечную точку для балки (обновлённую)
            beam.Modify(); //обновляем балку
        }
        public static void moveToZcolumn(Beam beam, double z) //метод перемещения начала колонны с плохими координами на координату Z
        {
            Point sPointNew = new Point(); //создаём новый объект типа точка для новой стартовой точки балки

            Point sPointOld = new Point(); //создаём новый объект типа точка для старой стартовой точки балки

            sPointOld = beam.StartPoint; //назначаем старую стартовую точку

            sPointNew.X = sPointOld.X; //назначаем координату X для стартовой точки
            sPointNew.Y = sPointOld.Y; //назначаем координату Y для стартовой точки
            sPointNew.Z = z + 100; //назначаем координату Z для стартовой точки (+100мм)

            beam.StartPoint = sPointNew; //назначаем стартовую точку для балки (обновлённую)

            beam.Modify(); //обновляем балку

            sPointNew.Z = z; //назначаем координату Z для стартовой точки

            beam.StartPoint = sPointNew; //назначаем стартовую точку для балки (обновлённую)
            beam.Modify(); //обновляем балку
        }
        public static void moveToZcplate(ContourPlate cplate, double z) //метод перемещения контурной пластины плохими точками на координату Z
        {
            foreach (ContourPoint cPoint in cplate.Contour.ContourPoints) //для каждой точки назначаем новую координату по z+100мм
            {
                cPoint.Z = z + 100;//присваиваем значение координаты
            }
            cplate.Modify();//модифицируем контурную пластину

            foreach (ContourPoint cPoint in cplate.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                cPoint.Z = z;//присваиваем значение координаты
            }
            cplate.Modify();//модифицируем контурную пластину
        }
        public static void roundBeamCoord(Beam beam, int rValue) //метод округления координат начала и конца балки до указанной пользователем величины
        {
            Point sPointNew = new Point(); //создаём новый объект типа точка для новой стартовой точки балки
            Point ePointNew = new Point(); //создаём новый объект типа точка для новой конечной точки балки

            Point sPointOld = new Point(); //создаём новый объект типа точка для старой стартовой точки балки
            Point ePointOld = new Point(); //создаём новый объект типа точка для старой конечной точки балки

            sPointOld = beam.StartPoint; //получаем старую стартовую точку
            ePointOld = beam.EndPoint; //получаем старую конечную точку

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

            statusBarMessage(beam); //выводим значение об обработке в консоль
        }

        //метод округления координат пользовательских компонентов
        public static void roundCPartPoints(CustomPart cmp, int rValue, string coord)
        {
            Point pt1 = new Point(); //создаём точки для получения точек вставки
            Point pt2 = new Point(); //создаём точки для получения точек вставки

            // получаем точки вставки (текущие)
            bool pt = cmp.GetStartAndEndPositions(ref pt1, ref pt2);

            //округляем полученные точки вставки
            Point pt1round = roundPoint(pt1, rValue, coord);
            Point pt2round = roundPoint(pt2, rValue, coord);

            // перемещаем объект на 100 мм в сторону, чтобы потом нормально вернуть назад
            pt = cmp.SetInputPositions(new Point(pt1.X + 100, pt1.Y + 100, pt1.Z + 100), new Point(pt2.X + 100, pt2.Y + 100, pt2.Z + 100));
            cmp.Modify();

            // перемещаем объект на правильные округлённые точки
            pt = cmp.SetInputPositions(pt1round, pt2round);
            cmp.Modify();
            //вызываем методо сообщения о выполнении команды
            statusBarMessage(cmp);
        }

        //метод округления координат контурных пластин
        public static void roundCplatePoints(ContourPlate cplate, int rValue, string coord)
        {
            foreach (ContourPoint cPoint in cplate.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                cPoint.X = cPoint.X + 100;//присваиваем значение координаты
                cPoint.Y = cPoint.Y + 100;//присваиваем значение координаты
                cPoint.Z = cPoint.Z + 100;//присваиваем значение координаты
            }
            cplate.Modify();//модифицируем контурную пластину

            foreach (ContourPoint cPoint in cplate.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                //получаем значение координаты
                cPoint.X = cPoint.X - 100;
                cPoint.Y = cPoint.Y - 100;
                cPoint.Z = cPoint.Z - 100;

                //округляем текущую точку
                ContourPoint cpRound = roundContourPoint(cPoint, rValue, coord);

                //назначаем координаты округлённые
                cPoint.X = cpRound.X;
                cPoint.Y = cpRound.Y;
                cPoint.Z = cpRound.Z;

            }
            cplate.Modify();//модифицируем контурную пластину

            //вызываем методо сообщения о выполнении команды
            statusBarMessage(cplate);
        }

        // метод для округления координат точки по входному значению и строке координат округления
        public static Point roundPoint(Point pt, int rValue, string coordinates)
        {
            char[] charArray = coordinates.ToCharArray(); //дробим строку по каким координатам округляем

            Point ptres = new Point(); //создаём результирующую округлённую точку
            ptres = pt;
            //проходимся по массиву букв и сравниваем какие координаты округляем
            //округляем только нужные координаты
            foreach (char k in charArray)
            {
                switch (k)
                {
                    case 'X': // округляем координату по X
                        ptres.X = roundValue(pt.X, rValue);
                        break;
                    case 'Y': // округляем координату по Y
                        ptres.Y = roundValue(pt.Y, rValue);
                        break;
                    case 'Z': // округляем координату по Z
                        ptres.Z = roundValue(pt.Z, rValue);
                        break;
                }
            }

            return ptres;
        }

        // метод для округления координат контурной точки по входному значению и строке координат округления
        public static ContourPoint roundContourPoint(ContourPoint pt, int rValue, string coordinates)
        {
            char[] charArray = coordinates.ToCharArray(); //дробим строку по каким координатам округляем

            ContourPoint ptres = new ContourPoint(); //создаём результирующую округлённую точку

            //проходимся по массиву букв и сравниваем какие координаты округляем
            //округляем только нужные координаты
            ptres.X = pt.X;
            ptres.Y = pt.Y;
            ptres.Z = pt.Z;

            foreach (char k in charArray)
            {
                switch (k)
                {
                    case 'X': // округляем координату по X
                        ptres.X = roundValue(pt.X, rValue);
                        break;
                    case 'Y': // округляем координату по Y
                        ptres.Y = roundValue(pt.Y, rValue);
                        break;
                    case 'Z': // округляем координату по Z
                        ptres.Z = roundValue(pt.Z, rValue);
                        break;
                }
            }

            return ptres;
        }

        //метод округления точек контроллайн
        public static void roundControlLinePoints(ControlLine cline, int rValue, string coord)
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
            p1 = roundPoint(p1, rValue, coord);
            p2 = roundPoint(p2, rValue, coord);

            //назначаем координаты изначальной controlline
            cline.Line.Point1 = p1;
            cline.Line.Point2 = p2;

            //изменяем controlline
            cline.Modify();

            //вызываем метод о сообщения о выполнении команды
            statusBarMessage(cline);
        }

        //метод округления точек арматурной группы
        public static void roundRebarGroupStartEndPoints(RebarGroup rb, int rValue, string coord)
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
            p1 = roundPoint(p1, rValue, coord);
            p2 = roundPoint(p2, rValue, coord);

            //назначаем координаты изначальной арматурной группе
            rb.StartPoint = p1;
            rb.EndPoint = p2;
            #endregion

            #region Обрабатываем точки формы арматурного стержня
            Polygon plg = new Polygon(); //полигон для точек внутри формы арматурного стержня
            Polygon plgf = new Polygon(); //полигон для фиктивных точек арматурного стержня
            plg = rb.Polygons[0] as Polygon; //получаем полигон точек из арматурного стержня

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
                pt = roundPoint(pt, rValue, coord);
                plg.Points.Add(pt);
            }

            rb.Polygons.Clear();//чистим исходный полигон
            rb.Polygons.Add(plg); //добавляем полигон исходных точек
            #endregion

            //изменяем 
            rb.Modify();

            //вызываем метод о сообщения о выполнении команды
            statusBarMessage(rb);
        }

        //метод округления точек начальных и конечных простых балок
        public static void roundBeamPoints(Beam beam, int rValue, string coord)
        {
            Point p1 = beam.StartPoint;
            Point p2 = beam.EndPoint;

            beam.StartPoint = new Point(p1.X + 100, p1.Y + 100, p1.Z + 100);
            beam.EndPoint = new Point(p2.X + 100, p2.Y + 100, p2.Z + 100);

            p1 = roundPoint(p1, rValue, coord);
            p2 = roundPoint(p2, rValue, coord);

            beam.StartPoint = p1;
            beam.EndPoint = p2;

            beam.Modify();

            statusBarMessage(beam);
        }

        //метод округления точек для polybeam
        public static void roundPolyBeamPoints(PolyBeam pBeam, int rValue, string coord)
        {
            foreach (ContourPoint pPoint in pBeam.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                pPoint.X = pPoint.X + 100;//присваиваем значение координаты
                pPoint.Y = pPoint.Y + 100;//присваиваем значение координаты
                pPoint.Z = pPoint.Z + 100;//присваиваем значение координаты
            }
            pBeam.Modify();//модифицируем контурную пластину

            foreach (ContourPoint pPoint in pBeam.Contour.ContourPoints) //для каждой точки назначаем новую координату по z
            {
                //получаем значение координаты
                pPoint.X = pPoint.X - 100;
                pPoint.Y = pPoint.Y - 100;
                pPoint.Z = pPoint.Z - 100;

                //округляем текущую точку
                ContourPoint cpRound = roundContourPoint(pPoint, rValue, coord);

                //назначаем координаты округлённые
                pPoint.X = cpRound.X;
                pPoint.Y = cpRound.Y;
                pPoint.Z = cpRound.Z;

            }
            pBeam.Modify();//модифицируем полибалку

            //вызываем методо сообщения о выполнении команды
            statusBarMessage(pBeam);
        }

        //метод округления входных точек в плагины и компоненты
        public static void roundComponentInputPoints(Component cmp, int rValue, string coord)
        {
            var inputObjects = cmp.GetComponentInput(); //получаем входные объекты плагина
            ComponentInput cmpI = new ComponentInput(); //создаём новый объект входных данных

            foreach (var k in inputObjects)
            {
                InputItem im = k as InputItem; //распаковываем элемент как inputitem
                Type type = k.GetType(); //получаем тип объекта
                //если объект точка
                try
                {
                    Point pti = im.GetData() as Point; //распаковываем элемент, как точку
                    pti = roundPoint(pti, rValue, coord); //округляем точку
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
            statusBarMessage(cmp);//выводим сообщение
        }

        // метод сообщения о выполнении команды
        public static void statusBarMessage(ModelObject mo)
        {
            string type = mo.GetType().Name; //получаем имя элемента
            string guid = mo.Identifier.GUID.ToString(); //получаем guid
            string resultString = type + " " + guid + "- DONE!!!"; // клеем выводную строку
            Operation.DisplayPrompt(resultString); //выводим в консоль
        }

        //метод округления до нужной величины входного значения
        private static double roundValue(double value, int rValue)
        {
            double result = Math.Round(value / rValue, 0) * rValue;
            return result;
        }
    }
}