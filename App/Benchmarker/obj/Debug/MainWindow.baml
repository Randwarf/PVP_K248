   M S B A M L   `   `   `  ????F  BBenchmarker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BenchmarkerBenchmarker  5   	   G(clr-namespace:Benchmarker.MVVM.ViewModelBenchmarker.MVVM.ViewModel  5   	       Benchmarker.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Benchmarker     5   	   >	viewModel(clr-namespace:Benchmarker.MVVM.ViewModel     5   	   $	??600??5	   	   $	??920??6     ? WindowStyle$
  None=?5
   	    ? 
ResizeMode$ NoResize=?5   	   
??????  ? AllowsTransparency .  ? DataContext 5      .  (Benchmarker.MVVM.ViewModel.MainViewModel  5   
   .??5      ?? ???#272537 ?? CornerRadius$ 10??5      ._?5   
   ?  ? ColumnDefinitions 5      ?? 5      $	??200 ?6#   ?? 5       ? RowDefinitions 5      ?? 5      $??75 ?6    ?? 5      .+?5      ?? $??App Benchmark??6   $??Center=?5       $
??Left=?5!      
???????$??22(?5#      $??20,0,0,0q?5$      ?? 5&      $??1??6   .+?5'      	? $??	Benchmark??6   $??50??5(      
???????$??14(?5*         MenuButtonTheme#??[  5+       d? 	IsChecked$
 TrueY?5,      ??5-      ?? *BenchmarkViewCommand+	? 5.      $??History??6   $??50??5/      
???????$??14(?51      #??[  52      ??53      ?? *HistoryViewCommand+	? 54      $??Compare??6   $??50??55      
???????$??14(?57      #??[  58      ??59      ?? *CompareViewCommand+5:      ?? 5<      $??1??6   $??1??5=      $??10q?5>      ??5?      ?? *CurrentView+5@      5B      5C      