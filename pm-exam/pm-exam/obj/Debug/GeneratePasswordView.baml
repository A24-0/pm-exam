   M S B A M L   `   `   `  ����B  >pm-exam, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:pm_exampm_exam  5      "    pm_exam.GeneratePasswordView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'localclr-namespace:pm_exam     5      .��5	      �� $��10q�6   .+�5
   
   �� -   5          ��Name$  uppercaseCheckBox��5      $6��0Прописные латинские буквы��5      �� 5   
   -   5        $  lowercaseCheckBox��5      $4��.Строчные латинские буквы��5      �� 5   
   -   5        $  digitsCheckBox��5      $��
Цифры��5      �� 5   
   -   5        $  specialCharsCheckBox��5      $+��%Специальные символы��5      �� 5   
    �� Orientation$ 
Horizontal=�5      $��Center=�5      .+�5      �� $��Длина пароля: ��5      �� 5      -   5         ��Name$ passwordLengthTextBox��5      $��35��5      $��20��5       �� TextAlignment$ Center=�5      $��0q�5       $��8��5!      5"      �� 5#   
   -   5        $ generatedPasswordTextBox��5$      $	��350��5%      $��22��5&       �� 
IsReadOnly . $��10q�5(      �� 5)   
   -   5        $	��250��5*      $��22��5+      $��Center=�5,      $��Center=�5-      $��10q�5.      $-��'Сгенерировать пароль��5/      50      51      52      