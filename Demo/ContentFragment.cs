using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Yalantis.Com.Sidemenu.Interfaces;

namespace Demo
{
    public class ContentFragment : Android.Support.V4.App.Fragment, IScreenShotable
    {
        public static readonly string CLOSE = "Close";
        public static readonly string BUILDING = "Building";
        public static readonly string BOOK = "Book";
        public static readonly string PAINT = "Paint";
        public static readonly string CASE = "Case";
        public static readonly string SHOP = "Shop";
        public static readonly string PARTY = "Party";
        public static readonly string MOVIE = "Movie";

        private View containerView;
        protected ImageView mImageView;
        protected int res;
        private Bitmap bitmap;

        public static ContentFragment NewInstance(int resId)
        {
            ContentFragment contentFragment = new ContentFragment();
            Bundle bundle = new Bundle();

            bundle.PutInt("ContentResId", resId);
            contentFragment.Arguments = bundle;
            return contentFragment;
        }

        public Bitmap Bitmap => bitmap;

        public void TakeScreenShot()
        {
            Java.Lang.Thread thread = new Java.Lang.Thread(() =>
            {
                var bitMap = Bitmap.CreateBitmap(containerView.Width,
                        containerView.Height, Bitmap.Config.Argb8888);
                Canvas canvas = new Canvas(bitMap);
                containerView.Draw(canvas);
                bitmap = bitMap;
            });

            thread.Start();
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.containerView = view.FindViewById(Resource.Id.container);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            res = this.Arguments.GetInt("ContentResId");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_main, container, false);
            mImageView = (ImageView)rootView.FindViewById(Resource.Id.image_content);
            mImageView.Clickable = true;
            mImageView.Focusable = true;
            mImageView.SetImageResource(res);
            return rootView;
        }

    }
}