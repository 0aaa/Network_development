   M S B A M L   `   `   `  ????  ApiIntro0UI    Weather0UI.ShellView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   .??5      ?   ? ColumnDefinitions  5   
   ?? 5      ?? 5      .+?5	   
   &   System.Windows.Controls.DataGrid     AutoGenerateColumns .  ??6   ?? *PersonagesView+ ?? SelectedItem 6:   ?? *SelectedPersonage+   Columns 5
      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name??6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   ?? *Name+  5      $ 
Birth year??6%    69   ?? *	BirthYear+  5      $ 	Eye color??6%    68   ?? *
EyeColor+  5      $ Gender??6%    65   ?? *Gender+  5      $ 
Hair color??6%    69   ?? *	HairColor+  5      $ Height??6%    65   ?? *Height+  5      $
 Mass??6%    63   ?? *Mass+  5      $ 
Skin color??6%    69   ?? *	SkinColor+  5      $ 	Homeworld??6%    68   ?? *	Homeworld+5      ?? 5   
   $??1??6   .+?5      ?? ??6   ?? *PersonageQuantitiesArr+ ?? SelectedValue 6F   ?? *CurrentQuantity+  5      ??6   ?? *SelectedWeatherData.FSSV[0]+  5      ??6   ?? *WeatherDataView[2]+  5      ??6   ?? *WeatherDataView[3]+  5      ??6   ?? *WeatherDataView[4]+5      5      5      