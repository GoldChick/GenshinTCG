   M S B A M L   `   `   `  ����N  JGenshinTCGCardMaker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:GenshinTCGCardMakerGenshinTCGCardMaker  5   	       GenshinTCGCardMaker.Maker   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:GenshinTCGCardMaker     5   	     � Title$  Maker��5   	   $	��450��6   $	��800��6$    � 
ResizeMode$ NoResize=�60   .��5
      � .+�5   
   �� -   5        $��txtFolderContent��5      $��Center=�6-   $��200,10,100,370q�6H   $
��Wrap=�6`   $9��3Mod Name: {TODO} | Made by {TODO} | Version: {TODO}��6t   � 5   
   -   5         �Name$ buttonContent��5      $��300,40,10,340q�6&   .+�5      �� $��	0,0,440,0q�6   ${�False��6(   .��6:   ←6=   �� 5      -   5        $��currentSelect��5      $��Center=�6.   $��
40,0,180,0q�6I   $
��Wrap=�6]   $��当前选择：��6q   �� 5      $��300,0,140,0q�6   ${�False��6*   .��6<   ×6?   �� 5      $��
360,0,80,0q�6   ${�False��6)   .��6;   Edit6A   �� 5      $��
400,0,40,0q�6   ${�False��6)   .��6;   -6>   �� 5      $��	440,0,0,0q�6   .��6(   +6+   5      �� 5   
   -   5        $��listBoxFolderContent��5      $��300,80,10,10q�6.   $��False��6D   ��5      �� 5      .R�5      �� ���#a0a0a0$��1q�63    �� CornerRadius$ 10��6G   ���#2b3b45 �� Padding$ 10q�6n   $��5q�6{   $	��430��6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6/      ShadowDepth$ 5Q�6=      Opacity$	 0.5Q�6M   	   
BlurRadius$	 10Q�6[   ._�5      �� .+�5       �� $��16(�6>   $
��Bold"�6L   ���#d0d7dc��6(   �� *Name+6^   �� 5"      
 �� Orientation$
 Vertical=�6)   $��0,5,0,0q�6@   .+�5#   "   �� ��6/   �� *TagsA+��5$   &   �� 5%   *   .R�5'   .   �� 
����� �$ 3��6K   $��2q�6\   ._�5(   2   ��  �� Padding$	 3,0q�6M   
���������6<   �� 6[   5)   /   5*   +   ��5,   &   �� 5-   *   .R�5.   .   � 5/   +   51   #   �� 52   "   ��6/   �� *TagsB+��53   &   �� 54   *   .R�56   .   �� 
������$ 3��6P   $��2q�6a   ._�57   2   �� $	 3,0q�6M   
���������6<   �� 6[   58   /   59   +   ��5;   &   �� 5<   *   .R�5=   .   � 5>   +   5@   #   5A      �� 5D      $��12(�6E   
�������$
��Wrap=�6f   ��6(   �� *Description+5F      5G      5H      5I      5K      5M      5N      