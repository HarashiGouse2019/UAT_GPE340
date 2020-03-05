public interface IPlayable
{
    bool OnReloadButtonDown { get; set; }
    bool OnRollButtonDown { get; set; }
    bool OnShootButtonDown { get; set; }
    bool OnEquipWeaponButtonDown { get; set; }
    float OnChangeWeapon { get; set; }
    float OnMoveHorizontal { get; set; }
    float OnMoveVertical { get; set; }
}
