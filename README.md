Для воскрешения проекта необходимо :
  1.Наличие Microsoft SQL Server и Microsoft SQL Server Management Studio
  2.Запустив Microsoft SQL Server Management Studio, восстановить резервную копию базы данных. (Файл nnn на ветке master)
  3.Для того, чтобы программа не просто запустилась, но и работала, необходимо в файле MainWindow.xaml.cs внести изменения в строку подключения. 
  После "Data Source" написать свое имя сервера, на котором вы восстанвливали базу данных.
  4.Запустить проект через Aboba.exe, который находится в \Aboba\bin\Debug
