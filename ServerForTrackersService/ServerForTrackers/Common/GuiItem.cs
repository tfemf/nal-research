// Decompiled with JetBrains decompiler
// Type: Nal.ServerForTrackers.Common.GuiItem
// Assembly: ServerForTrackersService, Version=8.6.5.0, Culture=neutral, PublicKeyToken=null
// MVID: B190D0B6-C731-4F01-B9F7-6646C129DAD9
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\ServerForTrackersService.exe

using System;
using System.ComponentModel;
using System.Linq.Expressions;

#nullable disable
namespace Nal.ServerForTrackers.Common
{
  public class GuiItem : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void SetProperty<T>(ref T field, T value, Expression<Func<T>> propertyExpression) where T : IEquatable<T>
    {
      if (((object) field != null || (object) value == null) && ((object) field == null || field.Equals(value)))
        return;
      field = value;
      this.RaisePropertyChanged<T>(propertyExpression);
    }

    protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(GuiItem.ExtractPropertyName<T>(propertyExpression)));
    }

    public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
      return ((MemberExpression) propertyExpression.Body).Member.Name;
    }
  }
}
