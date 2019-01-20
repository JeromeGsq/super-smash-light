using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class BaseView<T> : UnityView
{
	[Space(10)]

	[SerializeField]
	private AppearanceType appearanceType = AppearanceType.Default;

	[SerializeField]
	private Ease ease = Ease.Linear;

	[SerializeField]
	private float duration = 0.2f;

	protected CanvasGroup CanvasGroup
	{
		get;
		set;
	}

	protected RectTransform RectTransform
	{
		get;
		set;
	}

	protected T ViewModel
	{
		get;
		set;
	}

	public override void Awake()
	{
		base.Awake();

		this.CanvasGroup = this.GetComponent<CanvasGroup>();
		this.RectTransform = this.GetComponent<RectTransform>();
		this.ViewModel = this.GetComponent<T>();
	}

	public override void Start()
	{
		base.Start();

		this.ShowView();
	}

	private void ShowView()
	{
		switch(appearanceType)
		{
			case AppearanceType.FadeIn:
				this.CanvasGroup.alpha = 0;
				this.CanvasGroup.DOFade(1, this.duration)
								.SetEase(this.ease);
				break;

			case AppearanceType.ModalFromBottom:
				this.RectTransform.DOAnchorPosY(-this.RectTransform.rect.height, 0);
				this.RectTransform.DOAnchorPosY(0, this.duration)
								  .SetEase(this.ease);
				break;

			default:
				this.CanvasGroup.alpha = 1;
				break;
		}
	}
}
