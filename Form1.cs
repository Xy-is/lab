using System;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using SolidWorks.Interop.swconst;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;

namespace Solidworks_Lab
{
    public partial class Form1 : Form
    {

        SldWorks SwApp;
        IModelDoc2 swModel;
        Feature swFeature;
        bool status;
        public Form1()
        {
            InitializeComponent();
        }

        bool pov1 = false;
        bool pov2 = false;
        bool pov3 = false;
        bool pov4 = false;

        // Текст "Длина ребра куба"
        private void label1_Click(object sender, EventArgs e) { } 

        // Размер ребра куба
        private void textBox1_TextChanged(object sender, EventArgs e) { } 

        // Текст "Шаг для дуг"
        private void label2_Click(object sender, EventArgs e) { } 

        // Шаг между дугами
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        
        // Текст "Радиус дуг"
        private void label4_Click(object sender, EventArgs e) { }

        // Запись значения радиуса дуг
        private void textBox3_TextChanged_1(object sender, EventArgs e) { }

        //Отступо от ребра
        private void label5_Click(object sender, EventArgs e) { }

        private void textBox4_TextChanged(object sender, EventArgs e)
        { }


        //Cчётчики для удаления эскизов и бобышки

        //Счётчик эскиза бобышки
        int gos1 = -1;
        //Счётчик бобышка-вытянуть
        int gos2 = 0;
        //Счётчик эскиза дуг
        int gos3 = 0;

        //Кнопка отвечающая за построение модели
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к solidworks");
                return;
            }
            if (SwApp.IActiveDoc == null)
            {
                SwApp.NewPart();
            }

            

            swModel = SwApp.IActiveDoc2;
            {
                //Изменение счётчиков
                gos1 = gos1 + 2;
                gos2++;
                gos3 = gos3 + 2;

                //Переменная куба
                double rebro = double.Parse(textBox1.Text) / 1000;

                //Переменная шага
                double shagum = double.Parse(textBox2.Text) / 1000;

                //Высота сегмента
                double visota = double.Parse(textBox3.Text) / 1000;

                //Отступ от ребра
                double otstup = double.Parse(textBox4.Text) / 1000;

                //Шаг который будет изменятся динамически
                double shag = 0;

                //Переменная в которой находится кол-во шагов
                double kolvoshag = rebro / shagum;

                double proverka = 0;

                double d = 1;

                //Построение куба
                swModel.ClearSelection2(true);
                swModel.SketchManager.CreateCornerRectangle(0, 0, 0, rebro, rebro, 0);
                swModel.ClearSelection2(true);
                swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, rebro, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, false);
                swModel.ClearSelection2(true);


                    //Переменная по которой определяется граница для дуг
                    proverka = visota + shag;

                    //Вариант когда дуга смотрит вверх
                    swModel.Extension.SelectByID2("Спереди", "PLANE", 0, 0, 0, false, 0, null, 0);
                    swModel.SketchManager.InsertSketch(true);
                    swModel.ClearSelection2(true);

                    //Построение дуг на ребре куба 
                    for (double i = 1; i < kolvoshag; i++)

                    {
                        
                        //Останавливается если дуга выйдет за пределы куба
                        if ((proverka + 1.5*shagum  ) > rebro) break;
                        else
                        {
                            
                            if (shag != 0)

                            {
                                swModel.SketchManager.Create3PointArc(0 + otstup, shag, 0, rebro - otstup, shag, 0, rebro / 2, proverka, 0);
                                d++;
                            shag++;
                        }

                            proverka = proverka + shagum;
                            shag = shagum * i;
                        
                        //Линии соеденяющие дуги
                            if ((d % 2 == 0))
                            {
                                swModel.SketchManager.CreateLine(rebro - otstup, shag - shagum, 0, rebro - otstup, shag, 0);
                                swModel.SketchManager.CreateLine(0 + otstup, shag - shagum, 0, 0 + otstup, shag, 0);
                            }

                        };
                    



                     }

                swModel.FeatureManager.FeatureCut4(true, false, false, 0, 0, 0.074, 0.074, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02,
                    false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

                swModel.FeatureManager.FeatureCut4(true, false, true, 1, 0, 0.192, 0.01, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);


                swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);

                }

                swModel.ClearSelection2(true);
            

        }
        
        //Кнопка отвечающая за удаление модели
        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к solidworks");
                return;
            } 
            if (SwApp.IActiveDoc == null)
            {
                SwApp.NewPart();
            }

            swModel = SwApp.IActiveDoc2;
            {
                swModel.Extension.SelectByID2("Вырез-Вытянуть" + gos2, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.Extension.SelectByID2("Вырез-Вытянуть" + gos2, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditDelete();
                swModel.Extension.SelectByID2("Эскиз" + gos3, "SKETCH", 0, 0, 0, false, 0, null, 0);
                swModel.Extension.SelectByID2("Эскиз" + gos3, "SKETCH", 0, 0, 0, false, 0, null, 0);
                swModel.EditDelete();
                swModel.Extension.SelectByID2("Бобышка-Вытянуть" + gos2, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.Extension.SelectByID2("Бобышка-Вытянуть" + gos2, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swModel.EditDelete();
                swModel.Extension.SelectByID2("Эскиз" + gos1, "SKETCH", 0, 0, 0, false, 0, null, 0);
                swModel.Extension.SelectByID2("Эскиз" + gos1, "SKETCH", 0, 0, 0, false, 0, null, 0);
                swModel.EditDelete();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
                try
                {
                    SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                }
                catch
                {
                    MessageBox.Show("Не удалось подключиться к solidworks");
                    return;
                }
                if (SwApp.IActiveDoc == null)
                {
                    SwApp.NewPart();
                }



                swModel = SwApp.IActiveDoc2;
                {
                    //Изменение счётчиков
                    gos1 = gos1 + 2;
                    gos2++;
                    gos3 = gos3 + 2;

                    //Переменная куба
                    double rebro = double.Parse(textBox1.Text) / 1000;

                    //Переменная шага
                    double shagum = double.Parse(textBox2.Text) / 1000;

                    //Высота сегмента
                    double visota = double.Parse(textBox3.Text) / 1000;

                    //Отступ от ребра
                    double otstup = double.Parse(textBox4.Text) / 1000;

                    //Шаг который будет изменятся динамически
                    double shag = 0;

                    //Переменная в которой находится кол-во шагов
                    double kolvoshag = rebro / shagum;

                    double proverka = 0;

                    double d = 1;

                    //Построение куба
                    swModel.ClearSelection2(true);
                    swModel.SketchManager.CreateCornerRectangle(0, 0, 0, rebro, rebro, 0);
                    swModel.ClearSelection2(true);
                    swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, rebro, 0, false, false, false, false, 0, 0, false, false, false, false, true, true, true, 0, 0, false);
                    swModel.ClearSelection2(true);


                    //Переменная по которой определяется граница для дуг
                    proverka = visota + shag;

                    //Вариант когда дуга смотрит вверх
                    swModel.Extension.SelectByID2("Спереди", "PLANE", 0, 0, 0, false, 0, null, 0);
                    swModel.SketchManager.InsertSketch(true);
                    swModel.ClearSelection2(true);

                    //Построение дуг на ребре куба 
                    for (double i = 1; i < kolvoshag; i++)

                    {

                        //Останавливается если дуга выйдет за пределы куба
                        if ((proverka + 1.5 * shagum) > rebro) break;
                        else
                        {

                            if (shag != 0)

                            {
                                swModel.SketchManager.Create3PointArc(0 + otstup, shag, 0, rebro - otstup, shag, 0, rebro / 2, proverka, 0);
                                d++;
                                
                            }

                            proverka = proverka + shagum;
                            shag = (shagum * i );
                        shagum = shagum + 0.002;





                        //Линии соеденяющие дуги
                        if ((d % 2 == 0))
                            {
                            Console.WriteLine(Convert.ToString(shag));
                            Console.WriteLine(Convert.ToString(shagum));
                            swModel.SketchManager.CreateLine(rebro - otstup, shag - shagum, 0, rebro - otstup, shag, 0);
                                swModel.SketchManager.CreateLine(0 + otstup, shag - shagum, 0, 0 + otstup, shag, 0);
                            }

                        };


                    }

                    swModel.FeatureManager.FeatureCut4(true, false, false, 0, 0, 0.074, 0.074, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02,
                        false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);

                    swModel.FeatureManager.FeatureCut4(true, false, true, 1, 0, 0.192, 0.01, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false);


                    swModel.ClearSelection2(true);
                    swModel.SketchManager.InsertSketch(true);

                }

                swModel.ClearSelection2(true);


            }
        }
    }

