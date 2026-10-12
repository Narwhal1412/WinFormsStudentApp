using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BLL;
using DAL;
namespace WinFormUI
{
    public class StudentEnrollmentsForm : Form
    {
        private readonly CourseService _courseService = new CourseService();
        private readonly StudentService _studentService = new StudentService();
        private readonly EnrollmentService _enrollmentService = new EnrollmentService();

        private readonly ComboBox cboCourse = new ComboBox();
        private readonly ComboBox cboStudent = new ComboBox();
        private readonly ComboBox cboMoveTo = new ComboBox();
        private readonly DataGridView grid = new DataGridView();

        public StudentEnrollmentsForm()
        {
            Text = "Student Enrollments";
            Width = 1100;
            Height = 500;

            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;

            foreach (var cbo in new[] { cboCourse, cboStudent, cboMoveTo })
            {
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                cbo.Width = 150;
            }

            var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, WrapContents = false };
            bar.Controls.Add(MakeLabel("Course:"));
            bar.Controls.Add(cboCourse);
            bar.Controls.Add(MakeLabel("Student:"));
            bar.Controls.Add(cboStudent);
            bar.Controls.Add(MakeButton("Add", (s, e) => Run(AddEnrollment)));
            bar.Controls.Add(MakeLabel("Move to:"));
            bar.Controls.Add(cboMoveTo);
            bar.Controls.Add(MakeButton("Update", (s, e) => Run(MoveEnrollment)));
            bar.Controls.Add(MakeButton("Delete", (s, e) => Run(RemoveEnrollment)));
            bar.Controls.Add(MakeButton("Refresh", (s, e) => LoadData()));

            Controls.Add(grid);
            Controls.Add(bar);

            cboCourse.SelectedIndexChanged += (s, e) => LoadData();
            Load += (s, e) => LoadLists();
        }

        private static Label MakeLabel(string text)
        {
            return new Label { Text = text, AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
        }

        private static Button MakeButton(string text, EventHandler onClick)
        {
            var button = new Button { Text = text };
            button.Click += onClick;
            return button;
        }

        private static void Bind(ComboBox cbo, object data, string display, string value)
        {
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.DataSource = data;
        }

        private void LoadLists()
        {
            Bind(cboCourse, _courseService.GetAll(), "CourseName", "CourseID");
            Bind(cboMoveTo, _courseService.GetAll(), "CourseName", "CourseID");
            Bind(cboStudent, _studentService.GetAll(), "StudentName", "StudentID");
            LoadData();
        }

        private void LoadData()
        {
            if (cboCourse.SelectedValue == null) return;
            grid.DataSource = _enrollmentService.GetStudentsByCourse(Convert.ToInt32(cboCourse.SelectedValue));
        }

        private Student SelectedStudent()
        {
            return grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as Student;
        }

        private void AddEnrollment()
        {
            _enrollmentService.Enroll(Convert.ToInt32(cboStudent.SelectedValue), Convert.ToInt32(cboCourse.SelectedValue));
        }

        private void MoveEnrollment()
        {
            var student = SelectedStudent();
            if (student == null) return;
            _enrollmentService.Move(student.StudentID, Convert.ToInt32(cboCourse.SelectedValue), Convert.ToInt32(cboMoveTo.SelectedValue));
        }

        private void RemoveEnrollment()
        {
            var student = SelectedStudent();
            if (student == null) return;
            _enrollmentService.Unenroll(student.StudentID, Convert.ToInt32(cboCourse.SelectedValue));
        }

        private void Run(Action action)
        {
            try
            {
                action();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
    }
}
