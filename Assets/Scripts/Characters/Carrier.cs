/*Clase: Carrier
*Descripción: Clase abstracta que representa un portador o entidad viviente en el juego.
* Contiene atributos de identificación (id, nombre, descripción) y un recurso de life (Life).
* Ofrece métodos para consultar su información y manipular su salud mediante daño o curación.
*Atributos:
*   - id: identificador único del portador.
*   - name: nombre del portador.
*   - description: descripción del portador.
*   - Life: recurso de life asociado al portador.
*/
public abstract class Carrier
{
    private readonly int id;
    private readonly string name;
    private readonly string description;

    public Life Life { get; }

    /*Constructor: Carrier
    *Descripción: Inicializa un portador con id, nombre, descripción y una instancia de life.
    *Parámetros:
    *   - id: identificador único del portador.
    *   - name: nombre del portador.
    *   - description: descripción del portador.
    *   - life: recurso de life asociado.
    */
    protected Carrier(int id, string name, string description, Life life)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.Life = life;
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
    *Descripción: Verifica si el portador está muerto (life vacía).
    *Retorna: true si Life está en 0, false en caso contrario.
    */
    public bool IsDead() => Life.IsEmpty();

    /*Método: ApplyDamage
    *Descripción: Aplica daño al portador reduciendo su life.
    *Parámetros:
    *   - amount: cantidad de daño a aplicar.
    */
    public void ApplyDamage(float amount)
    {
        if (amount <= 0f)
            return;
        Life.Spend(amount);
    }

    /*Método: Heal
    *Descripción: Restaura la life del portador en la cantidad indicada.
    *Parámetros:
    *   - amount: cantidad de life a recuperar.
    */
    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;
        Life.Recover(amount);
    }
}
