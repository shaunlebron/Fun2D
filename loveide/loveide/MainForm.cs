using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using loveide.Properties;
using Alsing.SourceCode;
using System.Diagnostics;

namespace loveide
{
    public partial class MainForm : Form
    {
        private NewGame newDialog = new NewGame();

        List<string> gameNames = new List<string>();

        const string windowTitle = "Fun2D";

        private string rootpath = null;
        private string gamespath = null;
        private string projectpath = null;
        private string filepath = null;
        private string projectname = null;
        private string commonpath = null;

        public MainForm()
        {
            InitializeComponent();
            initProjectSpace();
            initSyntaxBox();
            refreshWindowTitle();
            initBarStyle();
            initBookmarkStyle();

            syntaxBox.Document.RowParsed += new ParserEventHandler(Document_RowParsed);
            syntaxBox.Document.RowDeleted += new ParserEventHandler(Document_RowDeleted);
            syntaxBox.Document.BookmarkRemoved += new RowEventHandler(Document_BookmarkRemoved);
            syntaxBox.Document.BookmarkAdded += new RowEventHandler(Document_BookmarkAdded);
            syntaxBox.CaretChange += new EventHandler(syntaxBox_CaretChange);

            this.KeyPreview = true;
            this.KeyDown += (sender, e) =>
            {
                if (e.Control && e.KeyCode.ToString() == "S" && modified)
                    trySave();
            };

            //Application.Idle += delegate { picPreview.Invalidate(); };
        }

        void Document_RowDeleted(object sender, RowEventArgs e)
        {
            if (e.Row.Bookmarked)
                syntaxBox.Document.InvokeBookmarkRemoved(e.Row);
        }

        Dictionary<Row, Label> bookmarks = new Dictionary<Row, Label>();

        void clearBookmarks()
        {
            flowBookmarks.Controls.Clear();
            bookmarks.Clear();
            currentBookmark = null;
        }

        void Document_BookmarkAdded(object sender, RowEventArgs e)
        {
            if (bookmarks.ContainsKey(e.Row))
                return;

            var name = (string)e.Row.Tag;
            if (name == null)
                return;

            int rowindex = e.Row.Index;
            int insertindex = 0;
            while (true)
            {
                rowindex = syntaxBox.Document.GetPreviousBookmark(rowindex);
                if (rowindex >= e.Row.Index)
                    break;

                insertindex++;
            }

            var label = makeBookmarkLabel(e.Row);

            bookmarks.Add(e.Row, label);
            flowBookmarks.Controls.Add(label);
            flowBookmarks.Controls.SetChildIndex(label, insertindex);
        }

        Row currentBookmark = null;

        void selectBookmark(Row row)
        {
            if (row != currentBookmark)
            {
                if (row != null)
                {
                    var label = bookmarks[row];
                    label.BackColor = markBackSelect;
                    label.ForeColor = Color.White;
                    label.Invalidate();
                }

                if (currentBookmark != null)
                {
                    var label = bookmarks[currentBookmark];
                    var p = label.PointToClient(Control.MousePosition);
                    if (!label.ClientRectangle.Contains(p))
                    {
                        label.BackColor = markBack;
                        label.ForeColor = Color.Black;
                    }
                }

                currentBookmark = row;
            }
        }

        Color markBackSelect;
        Color markBack;

        void initBookmarkStyle()
        {
            markBackSelect = colorHex("81A0B8");
            markBack = colorHex("BED4E7");
        }

        Label makeBookmarkLabel(Row row)
        {
            var label = new Label()
            {
                Text = (string)row.Tag,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = markBack,
                ForeColor = Color.Black,
                Cursor = Cursors.Hand
            };
            label.Padding = new Padding(5);
            label.Font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);

            label.Click += delegate { syntaxBox.GotoLine(row.Index); };
            label.MouseMove += delegate
            {
                if (row != currentBookmark)
                {
                    label.BackColor = markBackSelect;//colorHex("597E9A");
                    label.ForeColor = Color.White;
                    label.Invalidate();
                }
            };
            label.MouseLeave += delegate
            {
                if (row != currentBookmark)
                {
                    label.BackColor = markBack;
                    label.ForeColor = Color.Black;
                    label.Invalidate();
                }
            };
            return label;
        }

        void Document_BookmarkRemoved(object sender, RowEventArgs e)
        {
            removeBookmark(e.Row);
        }

        void removeBookmark(Row r)
        {
            flowBookmarks.Controls.Remove(bookmarks[r]);
            bookmarks.Remove(r);
            if (r == currentBookmark)
            {
                currentBookmark = null;
                updateCaret();
            }
        }

        void syntaxBox_CaretChange(object sender, EventArgs e)
        {
            // find current bookmark
            updateCaret();
        }

        void updateCaret()
        {
            var row = syntaxBox.Caret.CurrentRow;
            var index = syntaxBox.Caret.CurrentRow.Index;
            bool inMark = false;
            int bindex;
            if (syntaxBox.Document[index].Bookmarked)
            {
                bindex = index;
                inMark = true;
            }
            else
            {
                bindex = syntaxBox.Document.GetPreviousBookmark(index);

                // tagged the row with the function name if applicable
                if (syntaxBox.Document[bindex].Bookmarked && bindex < index)
                {
                    inMark = true;
                }
            }

            // bookmarked row
            var brow = inMark ? syntaxBox.Document[bindex] : null;

            selectBookmark(brow);
        }

        void Document_RowParsed(object sender, RowEventArgs e)
        {
            var row = e.Row;
            var text = row.Text.Trim();
            bool bookmarked = false;

            /*
            if (text.Length == 0 && row.Tag != null && row.Bookmarked == false)
            {
                var d = syntaxBox.Document[0];
                // SyntaxBox bug fix

                row.Bookmarked = false;
                row.Tag = null;
                return;
            }
             * */

            if (text.StartsWith("function ") && text.EndsWith("()"))
            {
                var start = "function ".Length;
                var length = text.Length - 2 - start;
                var name = text.Substring(start, length).Trim();

                bookmarked = name.Length > 0;
                row.Tag = name;
                if (bookmarked && row.Bookmarked)
                {
                    bookmarks[row].Text = name;
                }
            }
            else row.Tag = null;

            if (bookmarked != row.Bookmarked)
                row.Bookmarked = bookmarked;
        }

        void initSyntaxBox()
        {
            var d = SyntaxDefinition.FromSyntaxXml(Resources.lua);

            syntaxBox.Document.Parser.Init(d);
            syntaxBox.BackColor = colorHex("F8FCFF");
            syntaxBox.LineNumberBackColor = colorHex("F8FCFF");
            syntaxBox.LineNumberBorderColor = syntaxBox.GutterMarginBorderColor;
            //syntaxBox.LineNumberForeColor = Color.LightGray;
            //syntaxBox.HighLightedLineColor = colorHex("81A0B8");
            //syntaxBox.HighLightActiveLine = true;
            //syntaxBox.SelectionBackColor = colorHex("E3FC8D");
            //syntaxBox.SelectionForeColor = Color.Black;

            syntaxBox.Document.Change += delegate { DocumentModified = true; };
        }

        Color colorHex(string s)
        {
            var r = Convert.ToInt32(s.Substring(0, 2), 16);
            var g = Convert.ToInt32(s.Substring(2, 2), 16);
            var b = Convert.ToInt32(s.Substring(4, 2), 16);
            return Color.FromArgb(r, g, b);
        }

        private void updateGameNames() {
            var dirs = Directory.GetDirectories(gamespath);
            var dirnames = new List<string>();

            foreach (var d in dirs)
                dirnames.Add(d.Substring(d.LastIndexOf(@"\") + 1));

            for (int i = 0; i < dirnames.Count; i++)
            {
                var d = dirnames[i];
                if (!gameNames.Contains(d))
                {
                    gameNames.Add(d);
                    var t = new ToolStripMenuItem();
                    t.Name = d;
                    t.Click += getTryOpenDelegate(d);
                    t.Text = d;
                    gamesToolStripMenuItem.DropDownItems.Add(t);
                }
            }

            foreach (var g in gameNames.ToArray())
                if (!dirnames.Contains(g))
                {
                    gameNames.Remove(g);
                    gamesToolStripMenuItem.DropDownItems.RemoveByKey(g);
                }
        }

        EventHandler getTryOpenDelegate(string name)
        {
            return delegate
            {
                if (modified)
                {
                    var result = MessageBox.Show("Do you want to save your current game?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                        trySave();
                    else if (result == DialogResult.Cancel)
                        return;
                }
                openGame(name);
            };
        }

        private void initProjectSpace()
        {
            rootpath = Path.GetDirectoryName(Application.ExecutablePath);
            gamespath = rootpath + "\\games";
            if (!Directory.Exists(gamespath))
                Directory.CreateDirectory(gamespath);

            commonpath = rootpath + "\\common";
            if (!Directory.Exists(commonpath))
            {
                MessageBox.Show("The \"common\" folder is missing!.  I need this to run the games!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void buttonSave_Click(object sender, EventArgs e)
        {
            trySave();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tryCreate();
        }

        void tryCreate()
        {
            if (modified)
            {
                if (projectname == null)
                {
                    trySave();
                    return;
                }

                var result = MessageBox.Show("Do you want save changes to your current game?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    trySave();
                else if (result == DialogResult.Cancel)
                    return;
            }
            if (showNewProjectDialog())
            {
                openGame(projectname);
            }
        }

        private void createProjectFiles(string game)
        {
            setGame(game);

            Directory.CreateDirectory(projectpath);

            var files = Directory.GetFiles(commonpath);

            foreach(var file in files)
            {
                string name = file.Substring(file.LastIndexOf("\\")+1);
                File.Copy(file, projectpath + "\\" + name);
            }
        }

        private bool showNewProjectDialog()
        {
            newDialog.NameField = "";
            updateGameNames();
            newDialog.UsedGameNames = gameNames;

            if (newDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    createProjectFiles(newDialog.GameName);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            return false;
        }

        string DocumentText
        {
            get { return syntaxBox.Document.Text; }
            set
            {
                syntaxBox.GotoLine(0);
                syntaxBox.ScrollIntoView();
                syntaxBox.Document.Text = value;
            }
        }

        bool modified = false;
        bool DocumentModified
        {
            get { return modified; }
            set
            {
                if (modified != value)
                {
                    modified = value;
                    refreshWindowTitle();
                }
                modified = value;
            }
        }

        void refreshWindowTitle()
        {
            this.Text = windowTitle + " - ";

            if (projectname != null)
                this.Text += projectname;
            else
                this.Text += "<Untitled>";

            if (modified)
                this.Text += "*";
        }

        private void saveMainFile()
        {
            File.WriteAllText(filepath, DocumentText);
            DocumentModified = false;
        }

        private void closeProject()
        {
            DocumentText = "";
            DocumentModified = false;

            clearBookmarks();
            clearProcesses();
            clearImages();

            setGame(null);
        }

        private void trySave()
        {
            if (filepath == null)
                if (!showNewProjectDialog())
                    return;
            saveMainFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trySave();
        }

        private void tryPaint()
        {
            if (filepath == null)
                if (!showNewProjectDialog())
                    return;
            saveMainFile();
            openPaint("blank");
        }

        List<Process> processes = new List<Process>();

        void clearProcesses()
        {
            processes.Clear();
        }

        private void openPaint(string name)
        {
            var p = new Process();

            p.StartInfo.FileName = "mspaint";
            p.StartInfo.Arguments = "\"" + projectpath + "\\" + name + ".png\"";
            p.EnableRaisingEvents = true;
            p.Exited += delegate
            {
                refreshImageList();
                processes.Remove(p);

            };
            p.Start();
            processes.Add(p);
        }



        private void tryPlay()
        {
            if (filepath == null)
                if (!showNewProjectDialog())
                    return;
            saveMainFile();
            play();
        }

        private void setGame(string game)
        {
            if (game == null)
            {
                projectname = null;
                projectpath = null;
                filepath = null;
            }
            else
            {
                projectname = game;
                projectpath = gamespath + "\\" + game;
                filepath = projectpath + "\\game.lua";
            }
            refreshWindowTitle();
        }

        private void openGame(string game)
        {
            closeProject();
            setGame(game);

            DocumentText = File.ReadAllText(filepath);
            DocumentModified = false;
            refreshImageList();
        }

        private void play()
        {
            System.Diagnostics.Process.Start(rootpath + "\\love.exe",String.Format("\"{0}\"",projectpath));
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            updateGameNames();
        }

        private void buttonPlay_MouseMove(object sender, MouseEventArgs e)
        {
            buttonPlay.Image = Resources.playover;
        }

        private void buttonPlay_MouseLeave(object sender, EventArgs e)
        {
            buttonPlay.Image = Resources.play;
        }

        private void buttonSave_MouseMove(object sender, MouseEventArgs e)
        {
            buttonSave.Image = Resources.saveover;
        }

        private void buttonSave_MouseLeave(object sender, EventArgs e)
        {
            buttonSave.Image = Resources.save;
        }
        private void buttonPaint_MouseMove(object sender, MouseEventArgs e)
        {
            buttonPaint.Image = Resources.paintover;
        }

        private void buttonPaint_MouseLeave(object sender, EventArgs e)
        {
            buttonPaint.Image = Resources.paint;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            tryPlay();
        }

        private void buttonPaint_Click(object sender, EventArgs e)
        {
            tryPaint();
        }

        private void listImages_DoubleClick(object sender, EventArgs e)
        {
            tryOpenImage();
        }

        void tryOpenImage()
        {
            if (listImages.SelectedItem == null)
                return;
            string name = (string)listImages.SelectedItem;
            openPaint(name);
        }
        

        Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
        Dictionary<string, DateTime> imageModifyDate = new Dictionary<string, DateTime>();

        void clearImages()
        {
            foreach (var p in images)
                p.Value.Dispose();
            images.Clear();

            imageModifyDate.Clear();
            listImages.Items.Clear();
        }
        private void refreshImageList()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { refreshImageList(); }));
                return;
            }

            if (filepath == null)
                return;

            var files = new List<string>();
            int i = projectpath.Length+1;
            foreach (var f in Directory.GetFiles(projectpath, "*.png"))
            {
                string name = f.Substring(i, f.Length - projectpath.Length - 5);
                //if (name != "blank")
                    files.Add(name);
            }

            foreach (var f in files)
                if (!images.Keys.Contains(f))
                    addImage(f);

            foreach (var f in images.Keys.ToArray())
                if (!files.Contains(f))
                    removeImage(f);

            foreach (var f in images.Keys.ToArray())
            {
                var date = File.GetLastWriteTime(projectpath + "\\" + f + ".png");
                if (imageModifyDate[f] < date)
                {
                    refreshImage(f);
                    imageModifyDate[f] = date;
                }
            }

            refreshImageSelection();
        }

        void removeImage(string name)
        {
            images[name].Dispose();
            previewImage = null;
            images.Remove(name);
            listImages.Items.Remove(name);
            listImages.SelectedIndex = -1;
        }

        void addImage(string name)
        {
            var bitmap = getImageFromFile(name);
            images.Add(name, bitmap);
            imageModifyDate.Add(name, File.GetLastWriteTime(projectpath + "\\" + name + ".png"));
            listImages.Items.Add(name);
        }

        Bitmap getImageFromFile(string name)
        {
            // COPY the bytes to memory, to prevent file access problems when editing
            var bytes = File.ReadAllBytes(projectpath + "\\" + name + ".png");
            var bitmap = new Bitmap(new MemoryStream(bytes));
            return bitmap;
        }

        void refreshImage(string name)
        {
            images[name].Dispose();
            previewImage = null;
            images[name] = getImageFromFile(name);
        }

        void renameImage(string oldname, string newname)
        {
            images.Add(newname, images[oldname]);
            images.Remove(oldname);

            imageModifyDate.Add(newname, imageModifyDate[oldname]);
            imageModifyDate.Remove(oldname);

            for (int i = 0; i < listImages.Items.Count; i++)
                if ((string)listImages.Items[i] == oldname)
                {
                    listImages.Items[i] = newname;
                    break;
                }

            listImages.Invalidate();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                var result = MessageBox.Show("Do you want to save your game before exiting?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    trySave();
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        SizeF getFitSize(RectangleF source, Rectangle dest)
        {
            var w1 = source.Width;
            var h1 = source.Height;
            var w2 = (float)dest.Width;
            var h2 = (float)dest.Height;

            float ratio = Math.Min(w2 / w1, h2 / h1);
            if (ratio > 1)
                ratio = 1;
            return new SizeF(ratio * w1, ratio * h1);
        }

        private void picPreview_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            if (previewImage != null)
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(previewImage, fitRect, imageBounds, GraphicsUnit.Pixel);
            }
        }

        Image previewImage;
        SizeF fitSize;
        RectangleF fitRect;
        RectangleF imageBounds;
        float imageScale;

        private void listImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshImageSelection();
        }

        private void refreshImageSelection()
        {
            var s = (string)listImages.SelectedItem;
            if (s == null)
            {
                previewImage = null;
                picPreview.Invalidate();
                return;
            }

            previewImage = images[s];

            var units = GraphicsUnit.Pixel;
            imageBounds = previewImage.GetBounds(ref units);
            updatePreviewSize();
            picPreview.Invalidate();
        }

        private void updatePreviewSize()
        {
            if (previewImage == null)
            {
                fitSize = SizeF.Empty;
                fitRect = RectangleF.Empty;
                imageScale = float.NaN;
            }
            else
            {
                fitSize = getFitSize(imageBounds, picPreview.ClientRectangle);
                fitRect = new RectangleF(new PointF(0f, 0f), fitSize);
                imageScale = imageBounds.Width / fitSize.Width;
            }
        }

        private void picPreview_SizeChanged(object sender, EventArgs e)
        {
            updatePreviewSize();
            //picPreview.Invalidate();
        }

        private void picPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (previewImage == null)
            {
                txtCoords.Text = "";
            }
            else
            {
                var x = (int)(e.X * imageScale);
                var y = (int)(e.Y * imageScale);
                txtCoords.Text = String.Format("{0}, {1}",x,y);
            }
        }

        private void picPreview_Click(object sender, EventArgs e)
        {
            if (txtCoords.Text.Length > 0)
                Clipboard.SetText(txtCoords.Text);
        }

        private void listImages_Click(object sender, EventArgs e)
        {
            refreshImageList();
        }

        Brush barBrush;
        Pen barPen;

        void initBarStyle()
        {
            barBrush = new SolidBrush(colorHex("DCEFFF"));
            barPen = new Pen(colorHex("83C0F0"));
            flowBookmarks.Padding = new Padding(5,8,0,0);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var r = flowBookmarks.ClientRectangle;
            var w = Resources.cornerleft.Width;

            g.FillRectangle(barBrush, r.Left, r.Top, r.Width, r.Height);
            g.DrawRectangle(barPen, r.Left, r.Top, r.Width-1, r.Height);

            //g.DrawImage(Resources.cornerleft, r.Left, r.Top);
            //g.DrawImage(Resources.cornerright, r.Right - w, r.Top);
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            flowBookmarks.Invalidate();
        }

        void updateMenu()
        {
            undoToolStripMenuItem.Enabled =
                syntaxBox.Document.UndoBuffer.Count > 0;

            redoToolStripMenuItem.Enabled = 
                syntaxBox.Document.UndoStep < syntaxBox.Document.UndoBuffer.Count;

            cutToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled =
                syntaxBox.Selection.SelLength > 0;

            pasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            updateMenu();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxBox.Paste();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxBox.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxBox.Redo();
        }

        private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntaxBox.ShowFind();
        }

        private void listImages_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                listImages.SelectedIndex = listImages.IndexFromPoint(e.X, e.Y);
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            openToolStripMenuItem.Enabled =
                renameToolStripMenuItem.Enabled =
                deleteToolStripMenuItem.Enabled =
                listImages.SelectedIndex != -1;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tryOpenImage();
        }

        RenameForm renameForm = new RenameForm();

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = (string)listImages.SelectedItem;
            renameForm.NewName = name;
            if (renameForm.ShowDialog() != DialogResult.OK)
                return;

            string newname = renameForm.NewName;
            File.Move(projectpath + "\\" + name + ".png",
                projectpath + "\\" + newname + ".png");

            renameImage(name, newname);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = (string)listImages.SelectedItem;
            var result = MessageBox.Show("Are you sure you want to delete \"" + name + "\"?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                File.Delete(projectpath + "\\" + name + ".png");
                removeImage(name);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string credits = @"
Fun2D is a simple environment for making simple games on Windows.

Credits:
LOVE 2D - the game framework library used by Fun2D
        - all the art used by Fun2D (the application icon, and the hills)
SyntaxBox.NET - syntax code editor for Visual Studio
DEVOtion IDE - a Love2D IDE that was used for reference
Avalanche - a TI-83 calculator game - Cavequake is a clone of this
Key's Comic - the in-game font used by Fun2D
Xenocode Post Build - application virtualizer for bundling .NET framework
Citizen Schools - a national program for middle school students that Fun2D was built for
MirthKit - a simple game framework; Fun2D's core design principles are borrowed from MirthKit/Finity Flight

Shaun Williams
www.Fun2D.com";

            MessageBox.Show(credits, "ABOUT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
        }
    }
}
