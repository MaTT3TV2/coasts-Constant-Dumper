using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace coasts__Constant_Dumper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DumpButton_Click(object sender, RoutedEventArgs e)
        {
            dynamic Lua = new DynamicLua.DynamicLua();
            Lua(@"
            function RunIronbrewDump(input)
                local OldTableConcat = table.concat
                local DumpedStuff = ''
                local VariableCount = 0

                function FormatConcat(argument)
                    return ('[' .. VariableCount .. '] = ' .. argument)
                end

                table.concat = function(argument)
                    VariableCount = (VariableCount + 1)

                    if VariableCount == 1 then
                        DumpedStuff = (DumpedStuff .. FormatConcat('relaxin\' on the beach.') .. '\n')
                    else
                        DumpedStuff = (DumpedStuff .. FormatConcat(OldTableConcat(argument)) .. '\n')
                    end

                    return OldTableConcat(argument)
                end

                local AMOrPM = (tonumber(os.date('%H')) >= 12 and 'PM') or 'AM'

                DumpedStuff = (DumpedStuff .. '--// coasts\' Constant Dumper - ' .. os.date('%m/%d/%Y @ %H:%M:%S ') .. AMOrPM .. '\n')
                local SuccessDump, ErrorDump = pcall(load(input))

                if SuccessDump then
                    return DumpedStuff
                else
                    return ErrorDump
                end
            end");

            var DumpOutput = Lua.RunIronbrewDump(CodeInputTextbox.Text);
            CodeInputTextbox.Text = DumpOutput.ToString();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CodeInputTextbox.Text = "";
        }
    }
}
