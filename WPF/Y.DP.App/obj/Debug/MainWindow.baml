   M S B A M L   `   `   `  ����C  ?Y.DP.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Y.DP.AppY.DP.App  5   	   =#clr-namespace:Y.DP.App.UserControlsY.DP.App.UserControls  5   	       Y.DP.App.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Y.DP.App     5   	   8position#clr-namespace:Y.DP.App.UserControls     5   	   $
��1200��5	   
   $	��800��5
   	   ���#0B0D18  � 
ResizeMode$  NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�5   	    � Title$ 
MainWindow��5   	   .��5      � -   5         �Name$ mainGrid��5       � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $	��10* �6   6+   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $	��200 �6#   61   �� 5      $��* �6#   6/   �� 5      $	��200 �6#   61   ��5      �� 5       �� ImageSource$: 4Resources/Image/Background/HeaderBackgroundImage.png��6   6b   .+�5      � $��0��6   .+�5       �� $��30��6   $	��Top=�6%   $��	10,15,0,0q�6=   	 �� CornerRadius$	 8��6Q   ���#10243C
 J� Opacity$	
 0.8Q�6w   ._�5"      �� $��设计开发环境��6   ���#22ded8$��Center=�6B   $��Center=�6_   $��微软雅黑)�6z   6�   5#      5$      � 5%      $��1��6   $��Center=�6#   $	��Top=�6@   .+�5&      �� $-��'无锡四号线安全联锁检修系统��6   ���#22ded8$��26(�6E   $��微软雅黑)�6S   6g   5'      � 5(      $��2��6    5)      �� 5*      6)   �� 5+      6)   .+�5-      � $��1��6    5.      �� 5/      6*   �� 50      6*   .+�52      � $��0��6    53      �� 54      61   �� 55      61   �� 56      61   .+�58      �� $��0��6!   $��-��61   $��30��6=   $��1q�6H      btnSmallStyle#��[  6T   ��6|   �� *smallCommand+6�   �� 59      $��1��6!   $	��口��61   $��1q�6?     btnBigStyle#��[ 6J   ��6p   �� *normalCommand+6�   �� 5:      $��2��6!   $��x��61   $��30��6=   $��1q�6H     btnCloseStyle#��[ 6T   ��6{   �� *closeCommand+6�   5;      � 5<      $��1��6   .+�5=      �� -   5         ��Name$ btnLogin��5=   !   $��60��63   $��30��6>     RoundedButtonStyle#��[ 6k   ��6J   �� *loginCommand+6k   5>      5?      5@      5A      � 5B   
   $��1��6   .+�5C      � $��10q�6   y�6    �� *
Position_V+��5D      �� 5E      $3 -/Resources/Image/Border/RegionBorderImage.png��6!   6_    5G      �� 5H      $��* �6$   61   �� 5I      $��3* �6$   62   .+�5K      � $	��130��6   $��28��6#   $
��Left=�6/   $	��Top=�6J   $��	85,-8,0,0q�6b   ��5L      �� 5M      $4 ./Resources/Image/Other/TabItemCheckedImage.png��6%   6d   .+�5O      �� $��具区停车列检库��6   $��微软雅黑)�6.   $��14(�6@   $��Center=�6N   $	��Top=�6k   $	��Top=�6�   
�������6�   5P      � 5Q      $��0��6   $��Right=�6%   .+�5R      � $	��500��6   $��10,10,10,10q�6+   ���#17AAAF.+�5S      � ���#0C1424$��2,2,2,2q�64    5T      �� 5U   "   62   �� 5V   "   62   .+�5X      +   %Y.DP.App.UserControls.PositionControl  -   5          Name$ 
position_1��5X   7   $��0��6K   $��
20,15,5,15q�6X   6n     5Y      -   5        $ 
position_2��5Y   7   $��1��6K   $��
20,15,5,15q�6X   6n   5[      � 5\      $��55��6   $	��120��6*   $
��Left=�67   $��
-26,10,0,0q�6R   
������  5]      �� 5^   "   62   �� 5_   "   62   �� 5`   "   62   .+�5b      � $��0��6#   $��0q�62   .+�5c   "   F� -   5         F�Name$ 
doorIcon_1��5c   Z   $��0��6'   $��20��64   $��25��6?    �� Stretch$
 Fill=�6K   �����pǧ���pǧ    �`8q     �`8q �����(\��@�����(\��@ u���� u�������pǧ���ԩ 0�8 �0o0�8 �0o��Ѡ�R��Ѡ�R��T �06� ��T �06� ��ѐԩ ��ѐԩ 0�8 ����Q�0@`������Q�0@ �Ѱ�7 �Ѱ�7`������Q�0@`��	
��� � �6   5d      � 5e      $��1��6#   $��5q�60   .+�5f   "   F� -   5        $ 
doorIcon_2��5f   Z   $��1��6'   $��20��64   $��25��6?   $
 Fill=�6K   �����pǧ���pǧ    �`8q     �`8q �����(\��@�����(\��@ u���� u�������pǧ���ԩ 0�8 �0o0�8 �0o��Ѡ�R��Ѡ�R��T �06� ��T �06� ��ѐԩ ��ѐԩ 0�8 ����Q�0@`������Q�0@ �Ѱ�7 �Ѱ�7`������Q�0@`��	
��� � �6   5g      � 5h      $��2��6#   $��5q�60   .+�5i   "   F� -   5        $ 
doorIcon_3��5i   Z   $��2��6'   $��20��64   $��25��6?   $
 Fill=�6K   �����pǧ���pǧ    �`8q     �`8q �����(\��@�����(\��@ u���� u�������pǧ���ԩ 0�8 �0o0�8 �0o��Ѡ�R��Ѡ�R��T �06� ��T �06� ��ѐԩ ��ѐԩ 0�8 ����Q�0@`������Q�0@ �Ѱ�7 �Ѱ�7`������Q�0@`��	
��� � �6   5j      5k      5l      5m      � 5n      $��1��6   .+�5o      � ���#17AAAF$��10q�62   .+�5p      � ���#0C1424$��2q�64    5q      �� 5r   "   $��30 �60   �� 5s   "   �� 5t   "   �� 5u   "   �� 5v   "   �� 5w   "   �� 5x   "   .+�5z      � $��30��6#   $��30��6.   
������ $��200,-30,0,0q�6S   .+�5{   "   F� -   5        $ 
doorIcon_4��5{   Z   $��2��6'   $��20��64   $��25��6?   $
 Fill=�6K   �����pǧ���pǧ    �`8q     �`8q �����(\��@�����(\��@ u���� u�������pǧ���ԩ 0�8 �0o0�8 �0o��Ѡ�R��Ѡ�R��T �06� ��T �06� ��ѐԩ ��ѐԩ 0�8 ����Q�0@`������Q�0@ �Ѱ�7 �Ѱ�7`������Q�0@`��	
��� � �6   5|      � 5}      $��0��6#   $��5,10,0,0q�60   
������ 6]   � 5~      $��1��6#   $��5,10,0,0q�60   ���#10243C 5   "   �� 5�   &   69   �� 5�   &   69   .+�5�   "     -	   5        $ 
position_3��5�   <   $��1��6Q   $��5q�6a   6n   5�      � 5�      $��2��6#   $��5,10,0,0q�60   ���#10243C 5�   "   �� 5�   &   69   �� 5�   &   69   .+�5�   "     -
   5        $ 
position_4��5�   ;   $��0��6O   $��5q�6`   6m     5�   "   -   5        $ 
position_5��5�   ;   $��1��6O   $��5q�6_   6l   5�      � 5�      $��3��6#   $��5,10,0,0q�60   ���#10243C 5�   "   �� 5�   &   69   �� 5�   &   69   .+�5�   "     -   5        $ 
position_6��5�   ;   $��0��6O   $��5q�6_   6l     5�   "   -   5        $ 
position_7��5�   ;   $��1��6O   $��5q�6_   6l   5�      � 5�      $��4��6#   $��5,10,0,0q�60   ���#10243C 5�   "   �� 5�   &   69   �� 5�   &   69   .+�5�   "     -   5        $ 
position_8��5�   ;   $��0��6O   $��5q�6_   6l     5�   "   -   5        $ 
position_9��5�   ;   $��1��6O   $��5q�6_   6l   5�      � 5�      $��5��6#   $��5,10,0,0q�60   ���#10243C 5�   "   �� 5�   &   69   �� 5�   &   69   .+�5�   "     -   5        $ position_10��5�   ;   $��0��6P   $��5q�6`   6m     5�   "   -   5        $ position_11��5�   ;   $��1��6P   $��5q�6`   6m   5�      � 5�      $��6��6#   $��5,10,0,0q�60   ���#10243C 5�   "   �� 5�   &   69   �� 5�   &   69   .+�5�   "     -   5        $ position_12��5�   ;   $��0��6P   $��5q�6`   6m     5�   "   -   5        $ position_13��5�   ;   $��1��6P   $��5q�6`   6m   5�      5�      5�      5�      5�      2   ,Y.DP.App.UserControls.PositionDetailsControl  5�      -   5          Name$ position_Details��5�   W   $��1��6q   $��0q�6~   y�6.   �� *Position_DetailsV+6�   5�      5�      5�      