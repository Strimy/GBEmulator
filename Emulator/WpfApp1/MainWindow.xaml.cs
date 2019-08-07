using Emulator.GB.Core;
using Emulator.GB.Core.Cartridge;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CPU _cpu;
        private GPU _gpu;

        private ManualResetEventSlim _mre = new ManualResetEventSlim();

        public ObservableCollection<BGTileVisual> Visual
        {
            get; set;
        } = new ObservableCollection<BGTileVisual>();

        public MainWindow()
        {
            InitializeComponent();
            _gpu = new GPU();
            _gpu.VBlank += Gpu_VBlank;
            _cpu = new CPU(_gpu);
            _cpu.SetMMU(new MMU());
            var rom = new ROM();
            _cpu.MMU.SetCartridge(rom);

            rom.LoadROM(File.ReadAllBytes("opus5.gb"));

            Task.Run(() =>
            {
                while (true)
                {
                    _mre.Wait();
                    _cpu.Step();
                }
            });

        }

        private void Gpu_VBlank(object sender, EventArgs e)
        {
            _mre.Reset();

            Dispatcher.Invoke(() =>
            {
                Visual.Clear();
                foreach (var item in _gpu.SelectedBackgroundTileData)
                {
                    BGTileVisual tileVisual = new BGTileVisual(item);
                    Visual.Add(tileVisual);
                }

                bgTiles.ItemsSource = Visual;
            });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mre.Set();
        }
    }

    public class BGTileVisual
    {
        private List<List<Rectangle>> _pixel = new List<List<Rectangle>>();
        public List<List<Rectangle>> Pixels => _pixel;

        public BGTileVisual(BGTile tile)
        {
            Span<byte> data = MemoryMarshal.Cast<BGTile, byte>(new BGTile[] { tile }.AsSpan());

            for (int i = 0; i < 8; i++)
            {
                List<Rectangle> line = new List<Rectangle>();
                var lineDataL = data[i];
                var lineDataH = data[i+1];

                for (int j = 0; j < 8; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Fill = Brushes.DarkGreen;
                    r.Width = 4;
                    r.Height = 4;
                    r.StrokeThickness = 0;

                    var pixelData = lineDataL >> (7 - j) & 0x1;
                    var pixelData2 = lineDataH >> (7 - j) & 0x1;

                    if (pixelData == 0 && pixelData2 == 0)
                        r.Fill = Brushes.Red;

                    line.Add(r);
                }
                _pixel.Add(line);
            }
        }
    }
}
