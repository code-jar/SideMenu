using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using static Yalantis.Com.Sidemenu.Util.ViewAnimator;
using Android.Views;
using Yalantis.Com.Sidemenu.Interfaces;
using Android.Support.V4.Widget;
using Yalantis.Com.Sidemenu.Model;
using System.Collections.Generic;
using System;
using Android.Support.V7.Widget;
using Android.Content.Res;

namespace Demo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity, IViewAnimatorListener
    {
        private DrawerLayout drawerLayout;
        private ActionBarDrawerToggle drawerToggle;
        private List<SlideMenuItem> list = new List<SlideMenuItem>();
        private ContentFragment contentFragment;
        private static Yalantis.Com.Sidemenu.Util.ViewAnimator viewAnimator;
        private int res = Resource.Drawable.content_music;
        private LinearLayout linearLayout;


        public void AddViewToContainer(View view)
        {
            linearLayout.AddView(view);
        }

        public void DisableHomeButton()
        {
            SupportActionBar.SetHomeButtonEnabled(false);
        }

        public void EnableHomeButton()
        {
            SupportActionBar.SetHomeButtonEnabled(true);
            drawerLayout.CloseDrawers();
        }

        public IScreenShotable OnSwitch(IResourceble slideMenuItem, IScreenShotable screenShotable, int position)
        {
            if (slideMenuItem.Name == ContentFragment.CLOSE)
            {
                return screenShotable;
            }
            else
            {
                return ReplaceFragment(screenShotable, position);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            contentFragment = ContentFragment.NewInstance(Resource.Drawable.content_music);
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, contentFragment)
                .Commit();

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawerLayout.SetScrimColor(Android.Graphics.Color.Transparent);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.left_drawer);
            linearLayout.SetOnClickListener(new ViewOnClick(v =>
            {
                drawerLayout.CloseDrawers();
            }));

            SetActionBar();
            CreateMenuList();
            viewAnimator = new Yalantis.Com.Sidemenu.Util.ViewAnimator(this, list, contentFragment, drawerLayout, this);
        }

        private void CreateMenuList()
        {
            SlideMenuItem menuItem0 = new SlideMenuItem(ContentFragment.CLOSE, Resource.Drawable.icn_close);
            list.Add(menuItem0);
            SlideMenuItem menuItem = new SlideMenuItem(ContentFragment.BUILDING, Resource.Drawable.icn_1);
            list.Add(menuItem);
            SlideMenuItem menuItem2 = new SlideMenuItem(ContentFragment.BOOK, Resource.Drawable.icn_2);
            list.Add(menuItem2);
            SlideMenuItem menuItem3 = new SlideMenuItem(ContentFragment.PAINT, Resource.Drawable.icn_3);
            list.Add(menuItem3);
            SlideMenuItem menuItem4 = new SlideMenuItem(ContentFragment.CASE, Resource.Drawable.icn_4);
            list.Add(menuItem4);
            SlideMenuItem menuItem5 = new SlideMenuItem(ContentFragment.SHOP, Resource.Drawable.icn_5);
            list.Add(menuItem5);
            SlideMenuItem menuItem6 = new SlideMenuItem(ContentFragment.PARTY, Resource.Drawable.icn_6);
            list.Add(menuItem6);
            SlideMenuItem menuItem7 = new SlideMenuItem(ContentFragment.MOVIE, Resource.Drawable.icn_7);
            list.Add(menuItem7);
        }

        private void SetActionBar()
        {
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            drawerToggle = new CusActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close)
            {
                layout = linearLayout
            };

            drawerLayout.AddDrawerListener(drawerToggle);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (drawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }
            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private IScreenShotable ReplaceFragment(IScreenShotable screenShotable, int topPosition)
        {
            res = res == Resource.Drawable.content_music ? Resource.Drawable.content_films : Resource.Drawable.content_music;
            View view = FindViewById(Resource.Id.content_frame);
            int finalRadius = Math.Max(view.Width, view.Height);
            var animator = ViewAnimationUtils.CreateCircularReveal(view, 0, topPosition, 0, finalRadius);
            animator.SetInterpolator(new Android.Views.Animations.AccelerateInterpolator());
            animator.SetDuration(Yalantis.Com.Sidemenu.Util.ViewAnimator.CircularRevealAnimationDuration);

            FindViewById(Resource.Id.content_overlay).Background = new Android.Graphics.Drawables.BitmapDrawable(Resources, screenShotable.Bitmap);
            animator.Start();
            ContentFragment contentFragment = ContentFragment.NewInstance(res);
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, contentFragment).Commit();

            return contentFragment;
        }


        public class CusActionBarDrawerToggle : ActionBarDrawerToggle
        {
            public LinearLayout layout;

            public CusActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, Android.Support.V7.Widget.Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes) : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes)
            {
            }

            public override void OnDrawerSlide(View drawerView, float slideOffset)
            {
                base.OnDrawerSlide(drawerView, slideOffset);
                if (slideOffset > 0.6 && layout.ChildCount == 0)
                    viewAnimator.ShowMenuContent();
            }

            public override void OnDrawerClosed(View view)
            {
                base.OnDrawerClosed(view);
                layout.RemoveAllViews();
                layout.Invalidate();
            }
            public override void OnDrawerOpened(View drawerView)
            {
                base.OnDrawerOpened(drawerView);
            }
        }

    }


    public class ViewOnClick : Java.Lang.Object, View.IOnClickListener
    {
        private readonly Action<View> onClick;

        public ViewOnClick(Action<View> action)
        {
            onClick = action;
        }

        public void OnClick(View v)
        {
            onClick?.Invoke(v);
        }
    }



}

