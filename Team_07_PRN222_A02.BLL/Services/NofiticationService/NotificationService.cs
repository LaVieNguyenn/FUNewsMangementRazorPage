using AutoMapper;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.Services.NotificationService
{


    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateNotification(CreateNotificationDTO createNotificationDTO)
        {
            var noti = _mapper.Map<Notification>(createNotificationDTO);
           await _unitOfWork.NotificationRepository.InsertAsync(noti);
           await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllNotification()
        {
           var list  = _unitOfWork.NotificationRepository.GetAllAsync()
                                    .OrderByDescending(x => x.CreatedAt).ToList();
            return _mapper.Map<IEnumerable<NotificationDTO>>(list);

        }
    }
}