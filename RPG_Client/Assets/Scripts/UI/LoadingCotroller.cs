using UnityEngine;
using System.Collections;

public class LoadingCotroller : MonoBehaviour {

    public static LoadingCotroller instance;
    public UISlider slider;
    public UILabel LProgress;
    public UILabel LDesc;
    private DefAction OnLoadingSuccess;
    private bool isLoading = false;
    private AsyncOperation operation;
    private float curProgress = 0;
    public void Awake()
    {
        instance = this;
    }

	void Update () {
        if (isLoading)
        {
            if (operation != null)
                curProgress = operation.progress;
            slider.value = curProgress;
            LProgress.text = (int)(curProgress * 100) + "%";
            if ((operation != null && operation.isDone) || curProgress > 0.99f)
            {
                if (OnLoadingSuccess != null) OnLoadingSuccess();
                isLoading = false;
                this.operation = null;
                curProgress = 0;
            }
        }
	}


    IEnumerator EndLoading()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        yield return 0;
    }


    public void ShowLoadinge(AsyncOperation operation, DefAction OnLoadingSuccess = null , string desc = "")
    {
        if(OnLoadingSuccess != null)
        {
            this.OnLoadingSuccess = OnLoadingSuccess;
        }
        slider.value = 0;
        isLoading = true;
        gameObject.SetActive(true);
        if (operation != null)
        {
            this.operation = operation;
        }
        LDesc.text = desc;
    }

    public void ShowLoadinge(DefAction OnLoadingSuccess = null)
    {
        if (OnLoadingSuccess != null)
        {
            this.OnLoadingSuccess = OnLoadingSuccess;
        }
        isLoading = false;
        this.operation = null;
        curProgress = 0;
        slider.value = 0;
        isLoading = true;
        gameObject.SetActive(true);
    }


    public void UpdateProgress(float progress, string desc = "")
    {
        this.curProgress = progress;
        LDesc.text = desc;
    }

    public void CloseProgress()
    {
        isLoading = false;
        this.operation = null;
        curProgress = 0;
        gameObject.SetActive(false);
    }
}
