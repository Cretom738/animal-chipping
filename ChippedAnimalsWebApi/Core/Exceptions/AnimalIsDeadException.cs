namespace Core.Exceptions
{
    public class AnimalIsDeadException : BadRequestException
    {
        public AnimalIsDeadException(long? animalId)
            : base($"Animal with id {animalId} has DEAD life status")
        {
        }
    }
}
