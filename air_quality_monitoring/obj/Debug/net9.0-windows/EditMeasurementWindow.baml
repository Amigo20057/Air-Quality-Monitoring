   M S B A M L   `   `   `  ����  air_quality_monitoring8    2air_quality_monitoring.Views.EditMeasurementWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$)  #Редагування запису��5   	   $	��400��6$   $	��400��61    � WindowStartupLocation$ CenterOwner=�6=   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� .+�5      �� $��Місце:��6   �� 5      ��6   �� *
Location+�� 5      $��Температура:��6   $��0,10,0,0q�6,   �� 5      ��6   �� *Temperature+�� 5      $��Вологість:��6   $��0,10,0,0q�6*   �� 5      ��6   �� *
Humidity+�� 5      $��CO₂:��6   $��0,10,0,0q�6$   �� 5      ��6   �� *
CO2Level+�� 5      $��	Дата:��6   $��0,10,0,0q�6%   �� 5       �� 
IsReadOnly$
 True��61   ��6   �� *	Timestamp+61   5      �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,10,0,0q�6W   .+�5      �� -   5        $
��ОК��5      $��80��6"   $��5q�6-    �� 	IsDefault$
 True��68   6I   �� 5      $��Скасувати��6   $��80��6)   $��5q�64    �� IsCancel$
 True��6?   5      5       5!      