using System.ComponentModel;

public class ViewModelHelper : INotifyPropertyChanged
{
    private string consoleOutput;
    public string ConsoleOutput
    {
        get { return consoleOutput; }
        set
        {
            consoleOutput = value;
            OnPropertyChanged(nameof(ConsoleOutput));
        }
    }

    private string unneededItemsCount;
    public string UnneededItemsCount
    {
        get { return unneededItemsCount; }
        set
        {
            unneededItemsCount = value;
            OnPropertyChanged(nameof(UnneededItemsCount));
        }
    }

    private string rupeeCount;
    public string RupeeCount
    {
        get { return rupeeCount; }
        set
        {
            rupeeCount = value;
            OnPropertyChanged(nameof(RupeeCount));
        }
    }

    private string heartPiecesCount;
    public string HeartPiecesCount
    {
        get { return heartPiecesCount; }
        set
        {
            heartPiecesCount = value;
            OnPropertyChanged(nameof(HeartPiecesCount));
        }
    }

    private string seaChartsCount;
    public string SeaChartsCount
    {
        get { return seaChartsCount; }
        set
        {
            seaChartsCount = value;
            OnPropertyChanged(nameof(SeaChartsCount));
        }
    }

    private string squatsLabel;
    public string SquatsLabel
    {
        get { return squatsLabel; }
        set
        {
            squatsLabel = value;
            OnPropertyChanged(nameof(SquatsLabel));
        }
    }

    private string curlsLabel;
    public string CurlsLabel
    {
        get { return curlsLabel; }
        set
        {
            curlsLabel = value;
            OnPropertyChanged(nameof(CurlsLabel));
        }
    }

    private string shrugsLabel;
    public string ShrugsLabel
    {
        get { return shrugsLabel; }
        set
        {
            shrugsLabel = value;
            OnPropertyChanged(nameof(ShrugsLabel));
        }
    }

    private string overheadPressLabel;
    public string OverheadPressLabel
    {
        get { return overheadPressLabel; }
        set
        {
            overheadPressLabel = value;
            OnPropertyChanged(nameof(OverheadPressLabel));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}