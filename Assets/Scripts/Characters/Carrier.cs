/*Clase: Carrier
*Descripción: Clase abstracta que representa un portador o entidad viviente en el juego.
* Contiene atributos de identificación (id, nombre, descripción) y un recurso de vida (Vida).
* Ofrece métodos para consultar su información y manipular su salud mediante daño o curación.
*Atributos:
*   - id: identificador único del portador.
*   - name: nombre del portador.
*   - description: descripción del portador.
*   - Vida: recurso de vida asociado al portador.
*/
public abstract class Carrier
{
    private readonly int id;
    private readonly string name;
    private readonly string description;

    public Life Vida { get; }

    /*Constructor: Carrier
    *Descripción: Inicializa un portador con id, nombre, descripción y una instancia de vida.
    *Parámetros:
    *   - id: identificador único del portador.
    *   - name: nombre del portador.
    *   - description: descripción del portador.
    *   - vida: recurso de vida asociado.
    */
    protected Carrier(int id, string name, string description, Life vida)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.Vida = vida;
    }

    /*Método: GetId
    *Descripción: Devuelve el identificador único del portador.
    *Retorna: int con el id.
    */
    public int GetId() => id;

    /*Método: GetName
    *Descripción: Devuelve el nombre del portador.
    *Retorna: string con el nombre.
    */
    public string GetName() => name;

    /*Método: GetDescription
    *Descripción: Devuelve la descripción del portador.
    *Retorna: string con la descripción.
    */
    public string GetDescription() => description;

    /*Método: IsDead
    *Descripción: Verifica si el portador está muerto (vida vacía).
    *Retorna: true si Vida está en 0, false en caso contrario.
    */
    public bool IsDead() => Vida.IsEmpty();

    /*Método: ApplyDamage
    *Descripción: Aplica daño al portador reduciendo su vida.
    *Parámetros:
    *   - amount: cantidad de daño a aplicar.
    */
    public void ApplyDamage(float amount)
    {
        if (amount <= 0f)
            return;
        Vida.Spend(amount);
    }

    /*Método: Heal
    *Descripción: Restaura la vida del portador en la cantidad indicada.
    *Parámetros:
    *   - amount: cantidad de vida a recuperar.
    */
    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;
        Vida.Recover(amount);
    }
}
