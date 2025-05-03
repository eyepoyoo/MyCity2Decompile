using UnityEngine;

public class AttachmentAudio : MonoBehaviour
{
	public bool _isPlayingSFX;

	private SpecialAbility _vehicleAttachment;

	private SpecialAbility_Helicopter _vehicleAttachmentHelicopter;

	private SpecialAbility_Hose _vehicleAttachmentHose;

	private SpecialAbility_Jet _vehicleAttachmentJet;

	private SpecialAbility_Siren _vehicleAttachmentSiren;

	private void Awake()
	{
		if (GetComponent<SpecialAbility>() != null)
		{
			_vehicleAttachment = GetComponent<SpecialAbility>();
		}
		if (GetComponent<SpecialAbility_Helicopter>() != null)
		{
			_vehicleAttachmentHelicopter = GetComponent<SpecialAbility_Helicopter>();
			Debug.Log("HELICOPTER ATTACHMENT FOUND");
		}
		if (GetComponent<SpecialAbility_Hose>() != null)
		{
			_vehicleAttachmentHose = GetComponent<SpecialAbility_Hose>();
			Debug.Log("HOSE ATTACHMENT FOUND");
		}
		if (GetComponent<SpecialAbility_Jet>() != null)
		{
			_vehicleAttachmentJet = GetComponent<SpecialAbility_Jet>();
			Debug.Log("JET ATTACHMENT FOUND");
		}
		if (GetComponent<SpecialAbility_Siren>() != null)
		{
			_vehicleAttachmentSiren = GetComponent<SpecialAbility_Siren>();
			Debug.Log("SIREN ATTACHMENT FOUND");
		}
	}

	private void Update()
	{
		if (_vehicleAttachment == null && _vehicleAttachmentHelicopter == null && _vehicleAttachmentHose == null && _vehicleAttachmentJet == null && _vehicleAttachmentSiren == null)
		{
			return;
		}
		if (_vehicleAttachment != null)
		{
		}
		if (_vehicleAttachmentHelicopter != null)
		{
			if (!_isPlayingSFX && _vehicleAttachmentHelicopter._pIsInUse)
			{
				SoundFacade._pInstance.PlayLoopingSFX("AttachmentHelicopter", base.transform, 0f);
				_isPlayingSFX = true;
				Debug.Log("PLAYING HELICOPTER SOUNDS");
			}
			if (!_vehicleAttachmentHelicopter._pIsInUse)
			{
				SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentHelicopter");
				_isPlayingSFX = false;
				Debug.Log("STOPPING HELICOPTER SOUNDS");
			}
		}
		if (_vehicleAttachmentHose != null)
		{
			if (!_isPlayingSFX && _vehicleAttachmentHose._pIsInUse)
			{
				SoundFacade._pInstance.PlayLoopingSFX("AttachmentHose", base.transform, 0f);
				_isPlayingSFX = true;
				Debug.Log("PLAYING HOSE SOUNDS");
			}
			if (!_vehicleAttachmentHose._pIsInUse)
			{
				SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentHose");
				_isPlayingSFX = false;
				Debug.Log("STOPPING HOSE SOUNDS");
			}
		}
		if (_vehicleAttachmentJet != null)
		{
			if (!_isPlayingSFX && _vehicleAttachmentJet._pIsInUse)
			{
				SoundFacade._pInstance.PlayLoopingSFX("AttachmentJet", base.transform, 0f);
				_isPlayingSFX = true;
				Debug.Log("PLAYING JET SOUNDS");
			}
			if (!_vehicleAttachmentJet._pIsInUse)
			{
				SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentJet");
				_isPlayingSFX = false;
				Debug.Log("STOPPING JET SOUNDS");
			}
		}
		if (_vehicleAttachmentSiren != null)
		{
			if (!_isPlayingSFX && _vehicleAttachmentSiren._pIsInUse)
			{
				SoundFacade._pInstance.PlayLoopingSFX("AttachmentSiren", base.transform, 0f);
				_isPlayingSFX = true;
				Debug.Log("PLAYING SIREN SOUNDS");
			}
			if (!_vehicleAttachmentSiren._pIsInUse)
			{
				SoundFacade._pInstance.StopAllLoopingAudioByGroupName("AttachmentSiren");
				_isPlayingSFX = false;
				Debug.Log("STOPPING SIREN SOUNDS");
			}
		}
	}
}
