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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Text = "Student Management";
            WindowState = FormWindowState.Maximized;

            var file = new ToolStripMenuItem("File");
            file.DropDownItems.Add("Exit", null, (s, e) => Close());

            var menu = new MenuStrip();
            menu.Items.Add(file);
            menu.Items.Add(MakeMenu("Students", "View Students", (s, e) => new CrudForm<Student>("Students", new StudentService()).Show()));
            menu.Items.Add(MakeMenu("Courses", "View Courses", (s, e) => new CrudForm<Course>("Courses", new CourseService()).Show()));
            menu.Items.Add(MakeMenu("Instructors", "View Instructors", (s, e) => new CrudForm<Instructor>("Instructors", new InstructorService()).Show()));
            menu.Items.Add(MakeMenu("Departments", "View Departments", (s, e) => new CrudForm<Department>("Departments", new DepartmentService()).Show()));
            menu.Items.Add(MakeMenu("Enrollments", "Student Enrollments", (s, e) => new StudentEnrollmentsForm().Show()));
            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        private static ToolStripMenuItem MakeMenu(string title, string itemText, EventHandler onClick)
        {
            var item = new ToolStripMenuItem(title);
            item.DropDownItems.Add(itemText, null, onClick);
            return item;
        }
    }
}
