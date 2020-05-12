namespace komal.sdk {
    public interface ILifeCycle {
        void OnInit();
        void OnPause();
        void OnResume();
        void OnDestroy();
        void OnUpdate();
    }
}
