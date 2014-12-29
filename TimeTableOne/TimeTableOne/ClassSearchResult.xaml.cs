using TimeTableOne.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// TODO: 検索結果ページをアプリ内検索に接続します。
// 検索結果ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234240 を参照してください

namespace TimeTableOne
{
    /// <summary>
    /// このページには、グローバル検索がこのアプリケーションに指定されている場合に、検索結果が表示されます。
    /// </summary>
    public sealed partial class ClassSearchResult : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// これは厳密に型指定されたビュー モデルに変更できます。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        /// <summary>
        /// NavigationHelper は、ナビゲーションおよびプロセス継続時間管理を
        /// 支援するために、各ページで使用します。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ClassSearchResult()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// このページには、移動中に渡されるコンテンツを設定します。前のセッションからページを
        /// 再作成する場合は、保存状態も指定されます。
        /// </summary>
        /// <param name="navigationParameter">このページが最初に要求されたときに
        /// <see cref="Frame.Navigate(Type, Object)"/> に渡されたパラメーター値。
        /// </param>
        /// <param name="pageState">前のセッションでこのページによって保存された状態の
        /// ディクショナリ。ページに初めてアクセスするとき、状態は null になります。</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var queryText = e.NavigationParameter as String;

            // TODO: アプリケーション固有の検索ロジックです。検索プロセスでは、
            //       結果カテゴリのリストを作成する必要があります。
            //
            //       filterList.Add(new Filter("<フィルター名>", <結果数>));
            //
            //       アクティブな状態で開始するには、3 番目の引数として true を渡すフィルターが最初
            //       のフィルター (通常は "All") のみであることが必要です。アクティブ フィルターの
            //       結果は以下の Filter_SelectionChanged で提供されます。

            var filterList = new List<Filter>();
            filterList.Add(new Filter("All", 0, true));

            // ビュー モデルを介して結果を通信します
            this.DefaultViewModel["QueryText"] = '\u201c' + queryText + '\u201d';
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }

        /// <summary>
        /// スナップ化されていない場合に RadioButton を使用してフィルターが選択されたときに呼び出されます。
        /// </summary>
        /// <param name="sender">選択された RadioButton インスタンス。</param>
        /// <param name="e">RadioButton がどのように選択されたかを説明するイベント データ。</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            var filter = (sender as FrameworkElement).DataContext;

            // CollectionViewSource に変更内容をミラー化します。
            // これは不要である可能性があります。
            if (filtersViewSource.View != null)
            {
                filtersViewSource.View.MoveCurrentTo(filter);
            }

            // 選択されたフィルターを確認します
            var selectedFilter = filter as Filter;
            if (selectedFilter != null)
            {
                // 対応する Filter オブジェクト内に結果をミラー化し、
                // いない場合に RadioButton 表現を使用して変更を反映できるようにします
                selectedFilter.Active = true;

                // TODO: this.DefaultViewModel["Results"] をバインド可能な Image、Title、および Subtitle の
                //       バインドできる Image、Title、Subtitle、および Description プロパティを持つアイテムのコレクションに設定します

                // 結果が見つかったことを確認します
                object results;
                ICollection resultsCollection;
                if (this.DefaultViewModel.TryGetValue("Results", out results) &&
                    (resultsCollection = results as ICollection) != null &&
                    resultsCollection.Count != 0)
                {
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                    return;
                }
            }

            // 検索結果がない場合は、情報テキストを表示します。
            VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        #region NavigationHelper の登録

        /// このセクションに示したメソッドは、NavigationHelper がページの
        /// ナビゲーション メソッドに応答できるようにするためにのみ使用します。
        /// 
        /// ページ固有のロジックは、
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// および <see cref="GridCS.Common.NavigationHelper.SaveState"/> のイベント ハンドラーに配置する必要があります。
        /// LoadState メソッドでは、前のセッションで保存されたページの状態に加え、
        /// ナビゲーション パラメーターを使用できます。

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// 検索結果の表示に使用できるフィルターの 1 つを表すビュー モデルです。
        /// </summary>
        private sealed class Filter : INotifyPropertyChanged
        {
            private String _name;
            private int _count;
            private bool _active;

            public Filter(String name, int count, bool active = false)
            {
                this.Name = name;
                this.Count = count;
                this.Active = active;
            }

            public override String ToString()
            {
                return Description;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get { return _count; }
                set { if (this.SetProperty(ref _count, value)) this.OnPropertyChanged("Description"); }
            }

            public bool Active
            {
                get { return _active; }
                set { this.SetProperty(ref _active, value); }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, _count); }
            }

            /// <summary>
            /// プロパティの変更を通知するためのマルチキャスト イベント。
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// プロパティが既に目的の値と一致しているかどうかを確認します。必要な場合のみ、
            /// プロパティを設定し、リスナーに通知します。
            /// </summary>
            /// <typeparam name="T">プロパティの型。</typeparam>
            /// <param name="storage">get アクセス操作子と set アクセス操作子両方を使用したプロパティへの参照。</param>
            /// <param name="value">プロパティに必要な値。</param>
            /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
            /// この値は省略可能で、
            /// CallerMemberName をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
            /// <returns>値が変更された場合は true、既存の値が目的の値に一致した場合は
            /// false です。</returns>
            private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
            {
                if (object.Equals(storage, value)) return false;

                storage = value;
                this.OnPropertyChanged(propertyName);
                return true;
            }

            /// <summary>
            /// プロパティ値が変更されたことをリスナーに通知します。
            /// </summary>
            /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
            /// この値は省略可能で、
            /// <see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var eventHandler = this.PropertyChanged;
                if (eventHandler != null)
                {
                    eventHandler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        }
    }
}
