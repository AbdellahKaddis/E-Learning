using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class ChatMessageService
    {
        private readonly ChatMessageRepository _chatMessageRepository;

        public ChatMessageService(ChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }
        public async Task<bool> AddMessageAsync(CreateChatMessageDTO message)
        {
            return await _chatMessageRepository.AddMessageAsync(message);
        }


        public async Task<List<ChatMessageDTO>> GetMessagesAsync(int studentId, int instructorId)
        {
            var messages = await _chatMessageRepository.GetMessagesAsync(studentId, instructorId);

            var messageDtos = messages.Select(m => new ChatMessageDTO
            {
                Id = m.Id,
                StudentId = m.StudentId,
                InstructorId = m.InstructorId,
                SenderType = m.SenderType,
                MessageText = m.MessageText,
                SentAt = m.SentAt
            }).ToList();

            return messageDtos;
        }

    }

}
