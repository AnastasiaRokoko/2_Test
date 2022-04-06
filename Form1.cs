using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Тест_для_сотрудников_в_сфере_ИБ
{
    public partial class Form1 : Form
    {   //счетчик вопросов - создаем целочисл переменную
        int question_count;
        //целочисл переменная-номер правильного ответа
        int correct_answers;
        //целочисл переменная-количество неправильных ответов
        int wrong_answers;
        string [] array;
        //количество правильных ответов
        int correct_answers_number;
        //номер выбранного ответа
        int selected_response;

        //текстовый файл, который программа будет считывать
        System.IO.StreamReader Read;
        public Form1()
        {
            InitializeComponent();
            this.Activated += Form1_Activated;
        }
        void Start()
        {
            //пишем корректную кодировку из файла, который должны считать
            var encoding = System.Text.Encoding.GetEncoding(65001);//в скобках - пишем кодировку UTF8
            try
            {   //чтение из файла. Этот файл будет находиться в файле нашей программы
                Read = new System.IO.StreamReader(
                System.IO.Directory.GetCurrentDirectory() + @"\t.txt", encoding);
                this.Text = Read.ReadLine();
                question_count=0;
                correct_answers=0;
                wrong_answers=0;
                array = new string[10];
            }
            catch(Exception)
            {
                MessageBox.Show("error 1");
            }
            _Question();
        }
        //создадим функцию, которая будет считывать вопрос
        void _Question()
        {   
            //считывание текста в переменную label1
            label1.Text = Read.ReadLine();
            radioButton1.Text = Read.ReadLine();
            radioButton2.Text = Read.ReadLine();
            radioButton3.Text = Read.ReadLine();

            correct_answers = int.Parse(Read.ReadLine());
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

            //далее следует сделать кнопку "следующий вопрос" неактивной до того
            //момента, пока пользователь не выберет вариант ответа из предложенных
            button1.Enabled = false;
            question_count += 1;         
            if (label1.Text == "5/6. Какая веб-страница Сбербанка России является оригинальной?")
            {

                Pic1.Visible = true;
                Pic2.Visible = true;
                Pic3.Visible = true;
            }
            //когда вопросы закончились, меняем название кнопки
            if (Read.EndOfStream == true) button1.Text = "Завершить";
        }
        void _switching_status(object sender, EventArgs e)
        {   
            //кнопка активна
            button1.Enabled = true;
            button1.Focus();
            RadioButton Switcher = (RadioButton)sender;
            var tmp = Switcher.Name;
           
            //выясняем номер выбранного пользователем ответа
            selected_response = int.Parse(tmp.Substring(11)); 
        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            button1.Text = "Следующий вопрос";
            button2.Text = "Выход";

            radioButton1.CheckedChanged += new EventHandler(_switching_status);
            radioButton2.CheckedChanged += new EventHandler(_switching_status);
            radioButton3.CheckedChanged += new EventHandler(_switching_status);
            Start();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (selected_response == correct_answers)
            {
                correct_answers_number++;
            }
            if (selected_response != correct_answers)
            {
                wrong_answers++;
                //запоминаем вопросы с неправильным ответом, чтобы
                //потом их вывести пользователю
                array[wrong_answers] = label1.Text;
            }
            //реализуем возможность повторного запуска
         /*   if (button1.Text == "Начать тестирование заново")
            {
                button1.Text = "Следующий вопрос";
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                correct_answers_number = 0;
                question_count = 0;
                correct_answers = 0;
                wrong_answers = 0;
                Start();
                return;

            }
         */
            if (button1.Text == "Завершить")
            {
                Read.Close();
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;

                //вывод среднего балла за тест и количество правильных ответов
                label1.Text = string.Format("Тестирование завершено.\n" +
                   "Правильных ответов: {0} из {1}.\n" +
                    "Набранные баллы: {2:F2}.", correct_answers_number, question_count, (correct_answers_number * 5.0F) / question_count);
               
       //реализуем вывод вопросов, на которые 
       //пользователь ответил неверно
       var Str = "";
       for (int i = 1; i <= wrong_answers; i++)
           Str = Str + array[i] + "\n";

                //вывод списка вопросов
                if (wrong_answers != 0)
                {
                    MessageBox.Show(Str, "Вопросы с неправильными ответами");
                    do
                    {
                        Pic1.Visible = false;
                        Pic2.Visible = false;
                        Pic3.Visible = false;
                        radioButton1.Visible = true;
                        radioButton2.Visible = true;
                        radioButton3.Visible = true;
                        correct_answers_number = 0;
                        question_count = 0;
                        correct_answers = 0;
                        wrong_answers = 0;
                        Start();
                        return;
                    } while (wrong_answers != 0);
                    
                }
                if (wrong_answers == 0) button1.Visible = false;
            }
   if (button1.Text == "Следующий вопрос") _Question();
}

private void button2_Click_1(object sender, EventArgs e)
{
   //при нажатии на кнопку происходит выход из формы
   this.Close();
}
}
}
