using UnityEngine;
using System.Collections;

public class LoadingCotroller : MonoBehaviour {

    public static LoadingCotroller instance;
    public UISlider slider;
    public UILabel descLabel;
    private DefAction OnLoadingSuccess;
    private bool isLoading = false;
    private AsyncOperation operation;
    public void Awake()
    {
        instance = this;
    }

	void Update () {
        if (isLoading && operation !=null)
        {
            slider.value = operation.progress;
            descLabel.text = (int)(operation.progress * 100) + "%";
            if (operation.isDone)
            {
                if (OnLoadingSuccess != null) OnLoadingSuccess();
                isLoading = false;
                this.operation = null;
                StartCoroutine(EndLoading());
            }
        }
	}

    IEnumerator EndLoading()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        yield return 0;
    }


    public void ShowLoadinge(AsyncOperation operation, DefAction OnLoadingSuccess = null)
    {
        if(OnLoadingSuccess != null)
        {
            this.OnLoadingSuccess = OnLoadingSuccess;
        }
        if (!isLoading && operation != null)
        {
            this.operation = operation;
            isLoading = true;
            gameObject.SetActive(true);
        }
    }


}
