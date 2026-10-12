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
namespace WinFormUI
{
    public class CrudForm<T> : Form where T : class, new()
    {
        private readonly Service<T> _service;
        private readonly DataGridView grid = new DataGridView();
        private readonly PropertyGrid props = new PropertyGrid();
        private bool _isNew;

        public CrudForm(string title, Service<T> service)
        {
            _service = service;
            Text = title;
            Width = 1000;
            Height = 500;

            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.SelectionChanged += grid_SelectionChanged;

            props.Dock = DockStyle.Right;
            props.Width = 300;

            var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
            bar.Controls.Add(MakeButton("New", (s, e) => NewItem()));
            bar.Controls.Add(MakeButton("Save", (s, e) => SaveItem()));
            bar.Controls.Add(MakeButton("Delete", (s, e) => DeleteItem()));
            bar.Controls.Add(MakeButton("Refresh", (s, e) => LoadData()));

            Controls.Add(grid);
            Controls.Add(props);
            Controls.Add(bar);

            Load += (s, e) => LoadData();
        }

        private static Button MakeButton(string text, EventHandler onClick)
        {
            var button = new Button { Text = text };
            button.Click += onClick;
            return button;
        }

        private void LoadData()
        {
            grid.DataSource = _service.GetAll();
            _isNew = false;
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow != null)
            {
                props.SelectedObject = grid.CurrentRow.DataBoundItem;
                _isNew = false;
            }
        }

        private void NewItem()
        {
            props.SelectedObject = new T();
            _isNew = true;
        }

        private void SaveItem()
        {
            var item = props.SelectedObject as T;
            if (item == null) return;
            try
            {
                if (_isNew)
                {
                    _service.Add(item);
                }
                else
                {
                    _service.Update(item);
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        private void DeleteItem()
        {
            var item = props.SelectedObject as T;
            if (item == null || _isNew) return;
            if (MessageBox.Show("Delete this record?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try
            {
                _service.Delete(item);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }
    }
}
