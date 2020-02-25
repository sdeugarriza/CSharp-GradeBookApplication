using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GradeBook.Enums;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name) : base(name)
        {
            Type = GradeBookType.Ranked;
        }

        public override void CalculateStatistics()
        {
            if(Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a studen't overall grade.");
                return;
            }
            
            base.CalculateStatistics();
        }

        public override void CalculateStudentStatistics(string name)
        {
            if (Students.Count > 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a studen't overall grade.");
                return;
            }

            base.CalculateStudentStatistics(name);
        }

        public override char GetLetterGrade(double averageGrade)
        {
            if (Students.Count < 5)
                throw new InvalidOperationException("Ranked-grading requires a minimum of 5 students to work");

            var rankBoundaries = CalculateRanks();
            var match = rankBoundaries.FirstOrDefault(bound => averageGrade >= bound.LowerGrade);

            if (match == null)
                return 'F';

            return match.Grade;
        }

        private IList<RankBoundary> CalculateRanks()
        {
            var rankBoundaries = new List<RankBoundary>(5);
            var grades = new[] { 'A', 'B', 'C', 'D', 'F' };

            var studentCountPerRank = (int)Math.Round(Students.Count * 0.2);
            var studentsOrderedByGrade = Students.OrderByDescending(s => s.AverageGrade).ToList();

            int skip = 0;

            for (int i = 0; i < 5; i++)
            {
                var students = studentsOrderedByGrade.Skip(skip).Take(studentCountPerRank);
                rankBoundaries.Add(new RankBoundary
                {
                    UpperGrade = students.Max(s => s.AverageGrade),
                    LowerGrade = students.Min(s => s.AverageGrade),
                    Grade = grades[i]
                });

                skip += studentCountPerRank;
            }

            return rankBoundaries;
        }
    }
}
