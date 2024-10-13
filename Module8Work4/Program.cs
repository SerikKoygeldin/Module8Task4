
﻿using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryReadWrite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Student> studentsToWrite = new List<Student>
            {
                new Student { Name = "Жульен", Group = "G1", DateOfBirth = new DateTime(2001, 10, 22), AverageScore = 3.3M },
                new Student { Name = "Боб", Group = "G1", DateOfBirth = new DateTime(1999, 5, 25), AverageScore = 4.5M},
                new Student { Name = "Лилия", Group = "F2", DateOfBirth = new DateTime(1999, 1, 11), AverageScore = 5M},
                new Student { Name = "Роза", Group = "F2", DateOfBirth = new DateTime(1989, 9, 19), AverageScore = 3.7M}
            };

            WriteStudentsToBinFile(studentsToWrite, "C:\\CSharp\\students.dat");

            List<Student> studentsToRead = ReadStudentsFromBinFile("C:\\CSharp\\students.dat");

            foreach (Student studentProp in studentsToRead)
            {
                Console.WriteLine(studentProp.Name + " " + studentProp.Group + " " + studentProp.DateOfBirth + " " + studentProp.AverageScore);
            }

            try
            {    
                DirectoryInfo newDirectory = new DirectoryInfo(@"/Users/Serik/Desktop/Students");
                if (!newDirectory.Exists)
                    newDirectory.Create();

                foreach (Student studentProp in studentsToRead)
                {
                    FileInfo file = new FileInfo(@"/Users/Serik/Desktop/Students/" + studentProp.Group + ".txt");

                    if (!file.Exists)
                    {
                        using (StreamWriter sw = File.CreateText(file.FullName))  
                        {
                            sw.WriteLine(studentProp.Name + " " + studentProp.Group + " " + studentProp.DateOfBirth + " " + studentProp.AverageScore);                           
                        }                   
                    }
                    else 
                    {
                        var fileInfo = new FileInfo(file.FullName); 
                        StreamWriter sw = fileInfo.AppendText();

                        sw.WriteLine(studentProp.Name + " " + studentProp.Group + " " + studentProp.DateOfBirth + " " + studentProp.AverageScore);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void WriteStudentsToBinFile(List<Student> students, string fileName)
        {
            using FileStream fs = new FileStream(fileName, FileMode.Create);
            using BinaryWriter bw = new BinaryWriter(fs);

            foreach (Student student in students)
            {
                bw.Write(student.Name);
                bw.Write(student.Group);
                bw.Write(student.DateOfBirth.ToBinary());
                bw.Write(student.AverageScore);
            }
            bw.Flush();
            bw.Close();
            fs.Close();

        }

        static List<Student> ReadStudentsFromBinFile(string fileName)
        {
            List<Student> result = new();
            using FileStream fs = new FileStream(fileName, FileMode.Open);
            using StreamReader sr = new StreamReader(fs);

            Console.WriteLine(sr.ReadToEnd());

            fs.Position = 0;

            BinaryReader br = new BinaryReader(fs);

            while (fs.Position < fs.Length)
            {
                Student student = new Student();
                student.Name = br.ReadString();
                student.Group = br.ReadString();
                long dt = br.ReadInt64();
                student.DateOfBirth = DateTime.FromBinary(dt);
                student.AverageScore = br.ReadDecimal();

                result.Add(student);
            }

            fs.Close();
            return result;
        }
    }
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageScore { get; set; }
    }
}