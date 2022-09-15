namespace dotnet_rpg.DTOs.Fight
{
    public class AttackResultDto
    {
        public string Attacker { get; set; } = String.Empty;
        public string Opponent { get; set; } = String.Empty;
        public int AttackerHp { get; set; }
        public int OpponentHp { get; set; }
        public int Damage { get; set; }
    }
}
